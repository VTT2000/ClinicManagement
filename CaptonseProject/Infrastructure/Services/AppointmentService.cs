using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using web_api_base.Helper;
using web_api_base.Models.ClinicManagement;

public interface IAppointmentService
{
    public Task<dynamic> GetAllListPatientForDocTor(DateOnly date);
    public Task<HTTPResponseClient<List<AppointmentPatientVM>>> GetAllAppointmentPatientAsync();
    public Task<HTTPResponseClient<List<AppointmentPatientVM>>> GetAllAppointmentPatientForDateAsync(string date);
    public Task<HTTPResponseClient<bool>> CreateAppointmentFromReceptionist(AppointmentReceptionistCreateVM item);
}

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public AppointmentService(IUnitOfWork unitOfWork, JwtAuthService jwtAuthService)
    {
        _unitOfWork = unitOfWork;
    }

    // Implement methods for admin functionalities here

    public async Task<dynamic> GetAllListPatientForDocTor(DateOnly date){
        var result = new HTTPResponseClient<List<AppointmentPatientForDoctorVM>>();
        try
        {
            var list = await _unitOfWork._appointmentRepository.GetAllAppointmentPatientUserAsync(date);
            var data = list.Select(x => new AppointmentPatientForDoctorVM()
            {
                AppointmentId = x.AppointmentId,
                PatientId = x.PatientId,
                PatientFullName = x.Patient!.User!.FullName,
                AppointmentDate = x.AppointmentDate,
                Status = x.Status,
                Dob = x.Patient.Dob,
                Phone = x.Patient.Phone
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

    public async Task<HTTPResponseClient<List<AppointmentPatientVM>>> GetAllAppointmentPatientAsync()
    {
        var result = new HTTPResponseClient<List<AppointmentPatientVM>>();
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
            Console.WriteLine(ex.Message);
            result.Message = "Thất bại";
            result.StatusCode = StatusCodes.Status500InternalServerError;
        }
        result.DateTime = DateTime.Now;
        return result;
    }

    public async Task<HTTPResponseClient<List<AppointmentPatientVM>>> GetAllAppointmentPatientForDateAsync(string date)
    {
        var result = new HTTPResponseClient<List<AppointmentPatientVM>>();
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
                Console.WriteLine(ex.Message);
                result.Message = "Thất bại";
                result.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
        else
        {
            //Console.WriteLine($"Giá trị không hợp lệ, không thể chuyển sang DateTime.");
            result.Message = "Thất bại";
            result.StatusCode = StatusCodes.Status400BadRequest;
        }
        result.DateTime = DateTime.Now;
        return result;
    }

    public async Task<HTTPResponseClient<bool>> CreateAppointmentFromReceptionist(AppointmentReceptionistCreateVM item)
    {
        HTTPResponseClient<bool> result = new HTTPResponseClient<bool>();
        await _unitOfWork.BeginTransaction();
        try
        {
            Appointment data = new Appointment();
            data.DoctorId = item.DoctorId;
            data.AppointmentDate = item.AppointmentDate;
            data.Status = item.Status;
            if (item.PatientId == null)
            {
                User newUser = new User()
                {
                    FullName = item.PatientFullName,
                    Email = item.Email,
                    PasswordHash = PasswordHelper.HashPassword(item.PasswordHash),
                    Role = RoleConstant.Patient,
                };
                await _unitOfWork._userRepository.AddAsync(newUser);
                await _unitOfWork.SaveChangesAsync();
                Patient newPatient = new Patient()
                {
                    UserId = newUser.UserId,
                    Dob = item.Dob,
                    Phone = item.Phone,
                    Address = item.Address
                };
                await _unitOfWork._patientRepository.AddAsync(newPatient);
                await _unitOfWork.SaveChangesAsync();
                data.PatientId = newPatient.PatientId;
            }
            else
            {
                data.PatientId = item.PatientId;
            }
            await _unitOfWork._appointmentRepository.AddAsync(data);
            await _unitOfWork.SaveChangesAsync();
            
            await _unitOfWork.CommitTransaction();

            result.Message = "Thành công";
            result.StatusCode = StatusCodes.Status200OK;
            result.Data = true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBack();
            Console.WriteLine(ex.Message);
            result.Message = "Thất bại";
            result.StatusCode = StatusCodes.Status500InternalServerError;
            result.Data = false;
        }
        return result;
    }

}
