using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using web_api_base.Helper;
using web_api_base.Models.ClinicManagement;

public interface IWorkScheduleService
{
    // Task CreateAppointmentForPatient(Patient patient, User user, Appointment appointment);
    public Task<HTTPResponseClient<List<WorkScheduleDoctorVM>>> GetAllWorkScheduleDortorAsync();
}

public class WorkScheduleService : IWorkScheduleService
{
    private readonly IUnitOfWork _unitOfWork;

    public WorkScheduleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Implement methods for admin functionalities here

    public async Task<HTTPResponseClient<List<WorkScheduleDoctorVM>>> GetAllWorkScheduleDortorAsync()
    {
        var result = new HTTPResponseClient<List<WorkScheduleDoctorVM>>();
        try
        {
            var list = await _unitOfWork._workScheduleRepository.GetAllWorkScheduleDortorAsync();

            var data = list.Select(x => new WorkScheduleDoctorVM()
            {
                WorkScheduleId = x.WorkScheduleId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                DoctorName = x.Doctor!.User!.FullName
            }).ToList();
            result.Data = data;
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
