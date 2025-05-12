using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using web_api_base.Helper;
using web_api_base.Models.ClinicManagement;

public interface IWorkScheduleService
{
    // Task CreateAppointmentForPatient(Patient patient, User user, Appointment appointment);
    public Task<HTTPResponseClient<List<WorkScheduleDoctorVM>>> GetAllWorkScheduleDortorAsync();
    public Task<dynamic> GetWorkScheduleDortorAsync(int id);
    public Task<dynamic> DeleteWorkScheduleDortorAsync(int id);
    public Task<dynamic> SaveWorkScheduleDortorAsync(WorkScheduleDoctorDetailVM item);
}

public class WorkScheduleService : IWorkScheduleService
{
    private readonly IUnitOfWork _unitOfWork;

    public WorkScheduleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Implement methods for admin functionalities here

    public async Task<dynamic> SaveWorkScheduleDortorAsync(WorkScheduleDoctorDetailVM item)
    {
        var result = new HTTPResponseClient<bool>();
        result.Data = false;
        await _unitOfWork.BeginTransaction();
        try
        {
            var workSchedulefind = await _unitOfWork._workScheduleRepository.GetByIdAsync(item.WorkScheduleId);
            if (workSchedulefind == null)
            {
                // create
                await _unitOfWork._workScheduleRepository.AddAsync(new WorkSchedule(){
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    DoctorId = item.DoctorId
                });
            }
            else
            {
                // update
                workSchedulefind.StartDate = item.StartDate;
                workSchedulefind.EndDate = item.EndDate;
                workSchedulefind.StartTime = item.StartTime;
                workSchedulefind.EndTime = item.EndTime;
                workSchedulefind.DoctorId = item.DoctorId;
                _unitOfWork._workScheduleRepository.Update(workSchedulefind);
            }
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransaction();
            result.Data = true;
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

    public async Task<dynamic> DeleteWorkScheduleDortorAsync(int id)
    {
        var result = new HTTPResponseClient<bool>();
        result.Data = false;
        await _unitOfWork.BeginTransaction();
        try
        {
            var workSchedulefind = await _unitOfWork._workScheduleRepository.GetByIdAsync(id);
            if (workSchedulefind == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.Message = StatusCodes.Status400BadRequest.ToString();
            }
            else
            {
                await _unitOfWork._workScheduleRepository.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
                result.Data = true;
            }
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

    public async Task<dynamic> GetWorkScheduleDortorAsync(int id)
    {
        var result = new HTTPResponseClient<WorkScheduleDoctorDetailVM>();
        try
        {
            var workSchedulefind = await _unitOfWork._workScheduleRepository.GetByIdAsync(id);
            if (workSchedulefind == null)
            {
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.Message = StatusCodes.Status400BadRequest.ToString();
            }
            else
            {
                result.Data = new WorkScheduleDoctorDetailVM()
                {
                    WorkScheduleId = workSchedulefind.WorkScheduleId,
                    StartDate = workSchedulefind.StartDate,
                    EndDate = workSchedulefind.EndDate,
                    StartTime = workSchedulefind.StartTime,
                    EndTime = workSchedulefind.EndTime,
                    DoctorId = workSchedulefind.DoctorId
                };
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
