using System.Text.Json;

public interface IDiagnosisServiceService
{
    public Task<dynamic> GetAllDiagnosisServiceForTechcian(PagedResponse<TechnicianConditionFilterParaclinical> condition);
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
        Console.WriteLine(JsonSerializer.Serialize(condition));
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
                ServiceName = p.Service.ServiceName,
                RoomName = p.Room!.RoomName,
                DoctorName = p.Diagnosis.Appointment.Doctor!.User!.FullName,
                FullNamePerformed = p.UserIdperformedNavigation!.FullName,
                CreatedAt = p.CreatedAt,
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
}