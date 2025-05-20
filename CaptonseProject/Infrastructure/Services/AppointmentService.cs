using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using web_api_base.Helper;
using web_api_base.Models.ClinicManagement;

public interface IAppointmentService
{
    public Task<dynamic> GetAllListPatientForDocTorAsync2(PagedResponse<ConditionFilterPatientForAppointmentDoctor> condition, string authorization);
    public Task<dynamic> GetAllAppointmentPatientAsync2(PagedResponse<ConditionFilterPatientForAppointmentReceptionist> condition);
    public Task<dynamic> ChangeStatusWaitingForPatient(int appointmentId);
    public Task<dynamic> GetAllFreeTimeAppointmentForDoctor(DateOnly date, int doctorId);
    public Task<dynamic> UpdateStatusAppointmentForDoctor(int appointmentId, string status);
    //public Task<dynamic> GetAllListPatientForDocTor(DateOnly date);
    public Task<HTTPResponseClient<bool>> CreateAppointmentFromReceptionist(AppointmentReceptionistCreateVM item);
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
    public async Task<dynamic> GetAllListPatientForDocTorAsync2(PagedResponse<ConditionFilterPatientForAppointmentDoctor> condition, string authorization)
    {
        var result = new HTTPResponseClient<PagedResponse<List<AppointmentPatientForDoctorVM>>>();
        result.Data = new PagedResponse<List<AppointmentPatientForDoctorVM>>();
        result.Data.PageNumber = condition.PageNumber;
        result.Data.PageSize = condition.PageSize;
        try
        {
            string token = authorization.Substring("Bearer ".Length);
            string fullName = _jwtAuthService.DecodePayloadToken(token);
            var user = await _unitOfWork._userRepository.SingleOrDefaultAsync(p => p.FullName == fullName
            && (RoleConstant.Doctor == p.Role || RoleConstant.Admin == p.Role));
            if (user == null)
            {
                // không có quyền
                result.Message = "Thất bại";
                result.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else
            {
                var doctor = await _unitOfWork._doctorRepository.SingleOrDefaultAsync(p => p.UserId == user.UserId);
                var list = await _unitOfWork._appointmentRepository.GetAllAppointmentPatientUserAsync(
                    p =>
                    (user.Role == RoleConstant.Admin || (doctor != null && doctor.DoctorId == p.DoctorId))
                    && (condition.Data == null || condition.Data.dateAppointment == null || condition.Data.dateAppointment == p.AppointmentDate)
                    && (condition.Data == null || string.IsNullOrWhiteSpace(condition.Data.status) || condition.Data.status == p.Status)
                );
                if (condition.Data != null && !string.IsNullOrWhiteSpace(condition.Data.searchNamePatient))
                {
                    list = list
                        .Where(p => p.Patient != null
                            && p.Patient.User != null
                            && StringHelper.IsMatchSearchKey(condition.Data.searchNamePatient, p.Patient.User.FullName))
                        .ToList();
                }
                var data = list.Select(x => new AppointmentPatientForDoctorVM()
                {
                    AppointmentId = x.AppointmentId,
                    PatientId = x.PatientId,
                    PatientFullName = x.Patient!.User!.FullName,
                    AppointmentDate = x.AppointmentDate,
                    AppointmentTime = x.AppointmentTime,
                    Status = x.Status,
                    Dob = x.Patient.Dob,
                    Phone = x.Patient.Phone
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

    public async Task<dynamic> ChangeStatusWaitingForPatient(int appointmentId)
    {
        HTTPResponseClient<bool> result = new HTTPResponseClient<bool>();
        await _unitOfWork.BeginTransaction();
        try
        {
            var found = await _unitOfWork._appointmentRepository.GetByIdAsync(appointmentId);
            if (found == null)
            {
                result.Message = "Thất bại";
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.Data = false;
            }
            else
            {
                if ((found.Status ?? "").Equals(StatusConstant.AppointmentStatus.Booked))
                {
                    found.Status = StatusConstant.AppointmentStatus.Waiting;
                    _unitOfWork._appointmentRepository.Update(found);
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransaction();
                    result.Message = "Thành công";
                    result.StatusCode = StatusCodes.Status200OK;
                    result.Data = true;
                }
                else
                {
                    result.Message = "Lỗi dữ liệu";
                    result.StatusCode = StatusCodes.Status500InternalServerError;
                    result.Data = false;
                }
            }
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBack();
            Console.WriteLine(ex.Message);
            result.Message = "Thất bại";
            result.StatusCode = StatusCodes.Status500InternalServerError;
            result.Data = false;
        }
        result.DateTime = DateTime.Now;
        return result;
    }

    public async Task<dynamic> GetAllFreeTimeAppointmentForDoctor(DateOnly date, int doctorId)
    {
        HTTPResponseClient<List<TimeOnly>> result = new HTTPResponseClient<List<TimeOnly>>();
        try
        {
            // Lấy lịch làm việc của bác sĩ -> startime endtime
            var listWorkSchedule = await _unitOfWork._workScheduleRepository.WhereAsync(x => x.DoctorId == doctorId
                && x.StartDate.HasValue && (date.CompareTo(x.StartDate.Value) >= 0)
                && x.EndDate.HasValue && (date.CompareTo(x.EndDate.Value) <= 0));
            if (listWorkSchedule.Count() == 0)
            {
                // Không có lịch làm việc
                result.Message = "Bác sĩ chưa có lịch làm việc";
                return result;
            }
            // Lấy lịch khám của bác sĩ đã có -> appointmentTime
            var listAppointment = await _unitOfWork._appointmentRepository.WhereAsync(x => x.AppointmentDate.HasValue ? (x.AppointmentDate.Value.CompareTo(date) == 0 && x.DoctorId == doctorId) : false);
            // Tạo các khung giờ theo lịch làm việc của bác sĩ và loại bỏ các khung giờ đã đặt
            // 10 phút / 1 bệnh nhân (10 phút đầu và 10 phút cuối kết luận nếu đi xét nghiệm tổng thật là 20 phút 1 bệnh nhân)
            // (end time - startime)/2/10 = số bệnh nhân 
            // số bệnh nhân cũng là số khung giờ đặt lịch
            var data = new List<TimeOnly>();
            foreach (var item in listWorkSchedule)
            {
                double s = (item.EndTime!.Value - item.StartTime!.Value).TotalMinutes / 20;
                TimeOnly temp = item.StartTime.Value;
                for (int i = 0; i < s; i++)
                {
                    if (!listAppointment.Any(p => p.AppointmentTime.HasValue && p.AppointmentTime.Value.CompareTo(temp) == 0))
                    {
                        data.Add(temp);
                    }
                    temp = temp.AddMinutes(10);
                }
            }
            result.Data = data;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            result.Message = "Thất bại";
            result.StatusCode = StatusCodes.Status500InternalServerError;
            result.Data = null;
        }
        result.DateTime = DateTime.Now;
        return result;
    }

    public async Task<dynamic> UpdateStatusAppointmentForDoctor(int appointmentId, string status)
    {
        HTTPResponseClient<bool> result = new HTTPResponseClient<bool>();
        var found = await _unitOfWork._appointmentRepository.GetByIdAsync(appointmentId);
        List<string> listStatus = new List<string>()
        {
            StatusConstant.AppointmentStatus.Turned,
            StatusConstant.AppointmentStatus.Processing,
            StatusConstant.AppointmentStatus.Diagnosed,
        };

        if (found == null || !listStatus.Contains(status))
        {
            result.StatusCode = StatusCodes.Status400BadRequest;
            result.Message = "Tham số không hợp lệ!";
            result.Data = false;
        }
        else
        {
            await _unitOfWork.BeginTransaction();
            try
            {
                found.Status = status;
                _unitOfWork._appointmentRepository.Update(found);
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
        }
        return result;
    }

    // public async Task<dynamic> GetAllListPatientForDocTor(DateOnly date)
    // {
    //     var result = new HTTPResponseClient<List<AppointmentPatientForDoctorVM>>();
    //     try
    //     {
    //         var list = await _unitOfWork._appointmentRepository.GetAllAppointmentPatientUserAsync(date);
    //         var data = list.Select(x => new AppointmentPatientForDoctorVM()
    //         {
    //             AppointmentId = x.AppointmentId,
    //             PatientId = x.PatientId,
    //             PatientFullName = x.Patient!.User!.FullName,
    //             AppointmentDate = x.AppointmentDate,
    //             AppointmentTime = x.AppointmentTime,
    //             Status = x.Status,
    //             Dob = x.Patient.Dob,
    //             Phone = x.Patient.Phone
    //         }).ToList();
    //         result.Data = data;
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex.Message);
    //         result.Message = "Thất bại";
    //         result.StatusCode = StatusCodes.Status500InternalServerError;
    //     }
    //     result.DateTime = DateTime.Now;
    //     return result;
    // }
    
    public async Task<dynamic> GetAllAppointmentPatientAsync2(PagedResponse<ConditionFilterPatientForAppointmentReceptionist> condition)
    {
        var result = new HTTPResponseClient<PagedResponse<List<AppointmentPatientVM>>>();
        try
        {
            var appointmentList = await _unitOfWork._appointmentRepository.GetAllAppointmentPatientDoctorUser(
                a =>
                // Trạng thái là Booked hoặc Waiting
                ((a.Status ?? "") == StatusConstant.AppointmentStatus.Booked || (a.Status ?? "") == StatusConstant.AppointmentStatus.Waiting)
                // Lọc theo trạng thái nếu có
                && (condition.Data == null || string.IsNullOrWhiteSpace(condition.Data.Status) || condition.Data.Status == a.Status)
                // Lọc theo ngày nếu có
                && (condition.Data == null || !condition.Data.dateAppointment.HasValue || condition.Data.dateAppointment == a.AppointmentDate)
            );

            // Lọc theo tên bệnh nhân (chỉ có thể làm sau ToList vì dùng StringHelper)
            if (condition.Data != null && !string.IsNullOrWhiteSpace(condition.Data.searchNamePatient))
            {
                appointmentList = appointmentList
                    .Where(p => p.Patient != null
                        && p.Patient.User != null
                        && StringHelper.IsMatchSearchKey(condition.Data.searchNamePatient, p.Patient.User.FullName))
                    .ToList();
            }

            var data = appointmentList.Select(x => new AppointmentPatientVM()
            {
                AppointmentId = x.AppointmentId,
                PatientId = x.PatientId,
                PatientFullName = x.Patient!.User!.FullName,
                DoctorId = x.DoctorId,
                DoctorFullName = x.Doctor?.User?.FullName ?? "",
                AppointmentDate = x.AppointmentDate,
                AppointmentTime = x.AppointmentTime,
                Status = x.Status,
                Dob = x.Patient.Dob,
                Phone = x.Patient.Phone
            }).ToList();

            result.Data = new PagedResponse<List<AppointmentPatientVM>>();
            result.Data.PageNumber = condition.PageNumber;
            result.Data.PageSize = condition.PageSize;
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


    public async Task<HTTPResponseClient<bool>> CreateAppointmentFromReceptionist(AppointmentReceptionistCreateVM item)
    {
        HTTPResponseClient<bool> result = new HTTPResponseClient<bool>();
        await _unitOfWork.BeginTransaction();
        try
        {
            var workScheduleDoctor = await _unitOfWork._workScheduleRepository.WhereAsync(p => p.DoctorId == item.DoctorId);
            if (item.AppointmentDate >= DateOnly.FromDateTime(DateTime.Now)
                && item.AppointmentTime >= TimeOnly.FromDateTime(DateTime.Now)
                && workScheduleDoctor.Any(p=>p.StartDate <= item.AppointmentDate && p.EndDate >= item.AppointmentDate)
                && workScheduleDoctor.Any(p=>p.StartTime <= item.AppointmentTime && p.EndTime >= item.AppointmentTime)
                && (item.Status.Equals(StatusConstant.AppointmentStatus.Booked) || item.Status.Equals(StatusConstant.AppointmentStatus.Waiting))
                )
            {
                Appointment data = new Appointment();
                data.DoctorId = item.DoctorId;
                data.AppointmentDate = item.AppointmentDate;
                data.AppointmentTime = item.AppointmentTime;
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
            else
            {
                result.Message = "Thất bại";
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.Data = true;
            }
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
