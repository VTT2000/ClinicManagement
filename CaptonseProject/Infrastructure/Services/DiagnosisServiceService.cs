using System.Text.Json;

public interface IDiagnosisServiceService
{
    public Task<dynamic> GetAllDiagnosisServiceForTechcian(PagedResponse<TechnicianConditionFilterParaclinical> condition);
    public Task<dynamic> GetInfoTestForTechcian(int diagnosisServiceID);
    public Task<dynamic> SaveInfoTestForTechcian(TechnicianTestInfoParaclinicalSeviceVM item, string authorization);
}

public class DiagnosisServiceService : IDiagnosisServiceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtAuthService _jwtAuthService;

    public DiagnosisServiceService(IUnitOfWork unitOfWork, JwtAuthService jwtAuthService)
    {
        _unitOfWork = unitOfWork;
        _jwtAuthService = jwtAuthService;
    }

    // Implement methods for admin functionalities here
    public async Task<dynamic> GetAllDiagnosisServiceForTechcian(PagedResponse<TechnicianConditionFilterParaclinical> condition)
    {
        var result = new HTTPResponseClient<PagedResponse<List<TechnicianParaclinical>>>();
        result.Data = new PagedResponse<List<TechnicianParaclinical>>();
        result.Data.PageNumber = condition.PageNumber;
        result.Data.PageSize = condition.PageSize;
        try
        {
            var list = await _unitOfWork._diagnosisServiceRepository.GetAllDiagnosisService_Diagnosis_Appointment_Patient_Doctor_User_Service_User_Room(
                p =>
                (!condition.Data!.SearchDate.HasValue || p.Diagnosis!.Appointment!.AppointmentDate == condition.Data.SearchDate.Value)
                &&
                (!condition.Data.ServiceID.HasValue || p.ServiceId == condition.Data.ServiceID.Value)
                &&
                p.Service.Type == TypeServiceConstant.Paraclinical
            );
            list = list.Where(p =>
                string.IsNullOrWhiteSpace(condition.Data!.SearchNamePatient) || StringHelper.IsMatchSearchKey(condition.Data.SearchNamePatient, p.Diagnosis.Appointment!.Patient!.User!.FullName)
            ).ToList();
            if (condition.Data!.IsTested.HasValue)
            {
                if (condition.Data!.IsTested.Value)
                {
                    list = list.Where(p => !string.IsNullOrWhiteSpace(p.ServiceResultReport) && p.UserIdperformed.HasValue && p.RoomId.HasValue).ToList();
                }
                else
                {
                    list = list.Where(p => string.IsNullOrWhiteSpace(p.ServiceResultReport) && !p.UserIdperformed.HasValue && !p.RoomId.HasValue).ToList();
                }
            }

            var data = list.Select(p => new TechnicianParaclinical()
            {
                DiagnosesServiceId = p.DiagnosesServiceId,
                FullNamePatient = p.Diagnosis.Appointment!.Patient!.User!.FullName,
                DobPatient = p.Diagnosis.Appointment.Patient.Dob,
                PhonePatient = p.Diagnosis.Appointment.Patient.Phone,
                ParaclinicalServiceID = p.ServiceId,
                ServiceName = p.Service.ServiceName,
                DoctorName = p.Diagnosis.Appointment.Doctor!.User!.FullName,
                AppointmentDate = p.Diagnosis.Appointment.AppointmentDate,
                ServiceResultReport = p.ServiceResultReport
            }).ToList();

            result.Data.TotalRecords = data.Count;
            result.Data.TotalPages = (int)Math.Ceiling((double)data.Count / result.Data.PageSize);
            result.Data.Data = data
            // lấy theo page
            .Skip(condition.PageSize * (condition.PageNumber - 1))
            .Take(condition.PageSize).ToList();
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

    public async Task<dynamic> GetInfoTestForTechcian(int diagnosisServiceID)
    {
        var result = new HTTPResponseClient<TechnicianTestInfoParaclinicalSeviceVM>();
        result.Data = new TechnicianTestInfoParaclinicalSeviceVM();
        try
        {
            var temp = await _unitOfWork._diagnosisServiceRepository.GetByIdAsync(diagnosisServiceID);
            string name = string.Empty;
            if (temp!.UserIdperformed.HasValue)
            {
                var temp2 = await _unitOfWork._userRepository.GetByIdAsync(temp.UserIdperformed.Value);
                name = temp2 == null ? string.Empty : temp2.FullName;
            }
            
            var data = new TechnicianTestInfoParaclinicalSeviceVM()
            {
                DiagnosesServiceId = temp!.DiagnosesServiceId,
                RoomID = temp.RoomId,
                ServiceID = temp.ServiceId,
                NameUserperformed = name,
                CreatedAt = temp.CreatedAt,
                ServiceResultReport = temp.ServiceResultReport
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
    public async Task<dynamic> SaveInfoTestForTechcian(TechnicianTestInfoParaclinicalSeviceVM item, string authorization)
    {
        var result = new HTTPResponseClient<bool>();
        result.Data = false;
        try
        {
            await _unitOfWork.BeginTransaction();

            string token = authorization.Substring("Bearer ".Length);
            string fullName = _jwtAuthService.DecodePayloadToken(token);
            var user = await _unitOfWork._userRepository.SingleOrDefaultAsync(p => p.FullName == fullName
            && RoleConstant.Technician == p.Role);

            var temp = await _unitOfWork._diagnosisServiceRepository.GetByIdAsync(item.DiagnosesServiceId);

            temp!.UserIdperformed = user!.UserId;
            temp.RoomId = item.RoomID;
            temp.CreatedAt = DateTime.Now;
            temp.ServiceResultReport = item.ServiceResultReport;

            _unitOfWork._diagnosisServiceRepository.Update(temp);
            await _unitOfWork.SaveChangesAsync();

            result.Data = true;
            result.Message = "Thành công";
            result.StatusCode = StatusCodes.Status200OK;
            
            await _unitOfWork.CommitTransaction();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBack();
            Console.WriteLine(ex.Message);
            result.Message = "Thất bại";
            result.StatusCode = StatusCodes.Status500InternalServerError;
        }
        result.DateTime = DateTime.Now;
        return result;
    }
}