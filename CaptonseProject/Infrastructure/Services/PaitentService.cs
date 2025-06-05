using Microsoft.EntityFrameworkCore;
using web_api_base.Models.ClinicManagement;
using web_api_base.ViewModel;

public interface IPatientService
{

  Task<HTTPResponseClient<ProfilePatientVM>> GetProfilePatientById(int id);

  Task<HTTPResponseClient<ProfilePatientVM>> UpdatePatientById(int id, UpdateProfilePatientVM newPatient, IFormFile file);

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