using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

public interface IAppointmentService
{
    // Task CreateAppointmentForPatient(Patient patient, User user, Appointment appointment);
    public Task<HTTPResponseClient<IEnumerable<AppointmentPatientVM>>> GetAllAppointmentPatientAsync();
    public Task<HTTPResponseClient<IEnumerable<AppointmentPatientVM>>> GetAllAppointmentPatientForDateAsync(string date);
}

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtAuthService _jwtAuthService;

    public AppointmentService(IUnitOfWork unitOfWork, JwtAuthService jwtAuthService)
    {
        _unitOfWork = unitOfWork;
        _jwtAuthService = jwtAuthService;
    }

    // Implement methods for admin functionalities here

    public async Task<HTTPResponseClient<IEnumerable<AppointmentPatientVM>>> GetAllAppointmentPatientAsync()
    {
        var result = new HTTPResponseClient<IEnumerable<AppointmentPatientVM>>();
        try
        {
            var appointmentList = await _unitOfWork._appointmentRepository.GetAllAppointmentForReceptionistAsync();
            var data = appointmentList.Select(x => new AppointmentPatientVM()
            {
                AppointmentId = x.AppointmentId,
                PatientId = x.PatientId,
                PatientFullName = x.Patient!.User!.FullName,
                DoctorId = x.DoctorId,
                DoctorFullName = x.Doctor?.User?.FullName ?? "",
                AppointmentDate = x.AppointmentDate,
                Status = x.Status,
                Dob = x.Patient.Dob,
                Phone = x.Patient.Phone
            }).ToList();
            result.Data = data;
        }
        catch (Exception ex)
        {
            result.Message = ex.Message;
            result.StatusCode = StatusCodes.Status500InternalServerError;
        }
        result.DateTime = DateTime.Now;
        return result;
    }

    public async Task<HTTPResponseClient<IEnumerable<AppointmentPatientVM>>> GetAllAppointmentPatientForDateAsync(string date)
    {
        var result = new HTTPResponseClient<IEnumerable<AppointmentPatientVM>>();
        if (DateTime.TryParse(date, out DateTime condition))
        {
            //Console.WriteLine($"Giá trị hợp lệ: {condition}");
            try
            {
                var appointmentList = await _unitOfWork._appointmentRepository.GetAllAppointmentForReceptionistAsync(condition);
                var data = appointmentList.Select(x => new AppointmentPatientVM()
                {
                    AppointmentId = x.AppointmentId,
                    PatientId = x.PatientId,
                    PatientFullName = x.Patient!.User!.FullName,
                    DoctorId = x.DoctorId,
                    DoctorFullName = x.Doctor?.User?.FullName ?? "",
                    AppointmentDate = x.AppointmentDate,
                    Status = x.Status,
                    Dob = x.Patient.Dob,
                    Phone = x.Patient.Phone
                }).ToList();
                result.Data = data;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
        else
        {
            //Console.WriteLine($"Giá trị không hợp lệ, không thể chuyển sang DateTime.");
            result.Message = "Giá trị không hợp lệ, không thể chuyển sang DateTime.";
            result.StatusCode = StatusCodes.Status400BadRequest;
        }
        result.DateTime = DateTime.Now;
        return result;
    }
}
