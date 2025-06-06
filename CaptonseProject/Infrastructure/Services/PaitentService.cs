using Microsoft.EntityFrameworkCore;
using web_api_base.Models.ClinicManagement;
using web_api_base.Models.ViewModel.Receptionist;
using web_api_base.ViewModel;

public interface IPatientService
{
  Task<HTTPResponseClient<ProfilePatientVM>> GetProfilePatientById(int id);
  Task<HTTPResponseClient<ProfilePatientVM>> UpdatePatientById(int id, UpdateProfilePatientVM newPatient, IFormFile file);
  public Task<dynamic> GetAllPatientForReceptionistAsync(PagedResponse<ReceptionistConditionFilterForSelectedPatient> pagedResponse);
  public Task<dynamic> GetPatientForReceptionistAsync2(int patientID);
}

public class PatientService : IPatientService
{
  private readonly IPatientRepository _patientRepository;
  private readonly IUserRepository _userRepository;
  private readonly IUnitOfWork _unitOfWork;



  public PatientService(IPatientRepository patientRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
  {
    _unitOfWork = unitOfWork;
    _patientRepository = patientRepository;
    _userRepository = userRepository;
  }

  public async Task<dynamic> GetPatientForReceptionistAsync2(int patientID)
  {
    var result = new HTTPResponseClient<ReceptionistPatientInfoVM>()
    {
      Data = new ReceptionistPatientInfoVM()
    };
    try
    {
      var list = await _unitOfWork._patientRepository.GetAllPatientUserAsync(p=>p.PatientId == patientID);
      var temp = list.FirstOrDefault();

      var data = new ReceptionistPatientInfoVM()
      {
        PatientId = temp!.PatientId,
        FullName = temp.User != null ? temp.User.FullName : "",
        Dob = temp.Dob,
        Phone = temp.Phone,
        Address = temp.Address,
        Email = temp.User != null ? temp.User.Email : "",
        PasswordHash = temp.User != null ? temp.User.PasswordHash : ""
      };
      
      result.Data = data;
      result.Message = "Thành công";
      result.StatusCode = StatusCodes.Status200OK;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
      result.Message = "Thất bại";
      result.StatusCode = StatusCodes.Status500InternalServerError;
    }
    result.DateTime = DateTime.Now;
    return result;
  }

  public async Task<dynamic> GetAllPatientForReceptionistAsync(PagedResponse<ReceptionistConditionFilterForSelectedPatient> pagedResponse)
  {
    var result = new HTTPResponseClient<PagedResponse<List<ReceptionistSelectedPatientVM>>>()
    {
      Data = new PagedResponse<List<ReceptionistSelectedPatientVM>>()
      {
        Data = new List<ReceptionistSelectedPatientVM>(),
        PageNumber = pagedResponse.PageNumber,
        PageSize = pagedResponse.PageSize
      }
    };
    try
    {
      var list = await _unitOfWork._patientRepository.GetAllPatientUserAsync(p => !pagedResponse.Data!.DOB.HasValue || (p.Dob.HasValue && p.Dob.Value.CompareTo(pagedResponse.Data.DOB.Value) == 0));

      list = list.Where(p =>
      (string.IsNullOrWhiteSpace(pagedResponse.Data!.NamePatient) || (p.User != null && StringHelper.IsMatchSearchKey(pagedResponse.Data.NamePatient, p.User.FullName)))
      &&
      (string.IsNullOrWhiteSpace(pagedResponse.Data!.Phone) || (p.Phone != null && StringHelper.IsMatchSearchKey(pagedResponse.Data.Phone, p.Phone)))
      ).ToList();

      var data = list.Select(p => new ReceptionistSelectedPatientVM()
      {
        PatientId = p.PatientId,
        NamePatient = p.User != null ? p.User.FullName : "",
        DOB = p.Dob,
        Phone = p.Phone
      }).ToList();

      result.Data.TotalRecords = data.Count;
      result.Data.TotalPages = (int)Math.Ceiling((double)data.Count / result.Data.PageSize);
      result.Data.Data = data
      .Skip(result.Data.PageSize * (result.Data.PageNumber - 1))
      .Take(result.Data.PageSize).ToList();

      result.Message = "Thành công";
      result.StatusCode = StatusCodes.Status200OK;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
      result.Message = "Thất bại";
      result.StatusCode = StatusCodes.Status500InternalServerError;
    }
    result.DateTime = DateTime.Now;
    return result;
  }

  public async Task<HTTPResponseClient<ProfilePatientVM>> GetProfilePatientById(int id)
  {
    var result = new HTTPResponseClient<ProfilePatientVM>();
    try
    {
      var patient = await _userRepository.GetPatientById(id);

      if (patient == null)
      {
        result.StatusCode = 404;
        result.Message = "Patient not found";
        return result;
      }

      // Map dữ liệu vào ViewModel nếu cần
      result.StatusCode = 200;
      result.Data = patient;
      result.Message = "Get Patient Profile Successfully";
    }
    catch (Exception ex)
    {
      result.StatusCode = 500;
      result.Data = null;
      result.Message = $"Error: {ex.Message}";
    }

    return result;
  }

  public async Task<HTTPResponseClient<ProfilePatientVM>> UpdatePatientById(int id, UpdateProfilePatientVM newPatient, IFormFile file)
  {
    var result = new HTTPResponseClient<ProfilePatientVM>();
    try
    {
      var patient = await _userRepository.UpdatePatientById(id, newPatient, file);
      if (patient == null)
      {
        result.StatusCode = 404;
        result.Message = "Patient not found";
        return result;
      }

      result.StatusCode = 200;
      result.Data = patient;
      result.Message = "Update Patient Profile Successfully";
    }
    catch (Exception ex)
    {
      result.StatusCode = 500;
      result.Data = null;
      result.Message = $"Error: {ex.Message}";
    }

    return result;
  }
}