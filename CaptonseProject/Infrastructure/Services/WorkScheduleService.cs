using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using web_api_base.Helper;
using web_api_base.Models.ClinicManagement;

public interface IWorkScheduleService
{
    public Task<HTTPResponseClient<List<WorkScheduleDoctorVM>>> GetAllWorkScheduleDortorAsync();
    public Task<dynamic> GetAllWorkScheduleDortorAsync2(PagedResponse<ReceptionistConditionFilterWorkScheduleDoctor> pagedResponse);
    public Task<dynamic> GetWorkScheduleDortorAsync(int id);
    public Task<dynamic> DeleteWorkScheduleDortorAsync(int id);
    public Task<dynamic> SaveWorkScheduleDoctorAsync(WorkScheduleDoctorDetailVM item);
}

public class WorkScheduleService : IWorkScheduleService
{
    private readonly IUnitOfWork _unitOfWork;

    public WorkScheduleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Implement methods for admin functionalities here

    public async Task<dynamic> SaveWorkScheduleDoctorAsync(WorkScheduleDoctorDetailVM item)
    {
        var result = new HTTPResponseClient<bool>();
        result.Data = false;
        try
        {
            await _unitOfWork.BeginTransaction();
            if (item.WorkScheduleId.HasValue)
            {
                var found = await _unitOfWork._workScheduleRepository.GetByIdAsync(item.WorkScheduleId.Value);

                // update
                found!.StartDate = item.StartDate;
                found.EndDate = item.EndDate;
                found.StartTime = item.StartTime;
                found.EndTime = item.EndTime;
                found.DoctorId = item.DoctorId;
                _unitOfWork._workScheduleRepository.Update(found);
            }
            else
            {
                // create
                await _unitOfWork._workScheduleRepository.AddAsync(new WorkSchedule()
                {
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    DoctorId = item.DoctorId
                });
            }
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransaction();

            result.Data = true;
            result.Message = "Thành công";
            result.StatusCode = StatusCodes.Status200OK;
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
        try
        {
            await _unitOfWork.BeginTransaction();
            var found = await _unitOfWork._workScheduleRepository.GetByIdAsync(id);
            await _unitOfWork._workScheduleRepository.DeleteAsync(found!.WorkScheduleId);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransaction();
            
            result.Data = true;
            result.Message = "Thành công";
            result.StatusCode = StatusCodes.Status200OK;
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
            result.Data = new WorkScheduleDoctorDetailVM()
            {
                WorkScheduleId = workSchedulefind.WorkScheduleId,
                StartDate = workSchedulefind.StartDate,
                EndDate = workSchedulefind.EndDate,
                StartTime = workSchedulefind.StartTime,
                EndTime = workSchedulefind.EndTime,
                DoctorId = workSchedulefind.DoctorId
            };
                
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

    public async Task<dynamic> GetAllWorkScheduleDortorAsync2(PagedResponse<ReceptionistConditionFilterWorkScheduleDoctor> pagedResponse)
    {
        var result = new HTTPResponseClient<PagedResponse<List<WorkScheduleDoctorVM>>>();
        result.Data = new PagedResponse<List<WorkScheduleDoctorVM>>()
        {
            Data = new List<WorkScheduleDoctorVM>(),
            PageNumber = pagedResponse.PageNumber,
            PageSize = pagedResponse.PageSize
        };

        try
        {
            DateOnly conditionDate = new DateOnly();
            TimeOnly conditionTime = new TimeOnly();
            if (pagedResponse.Data!.WorkDate.HasValue)
            {
                conditionDate = pagedResponse.Data.WorkDate.Value;
            }
            if (pagedResponse.Data!.WorkTime.HasValue)
            {
                conditionTime = pagedResponse.Data.WorkTime.Value;
            }

            var list = await _unitOfWork._workScheduleRepository.GetAllWorkScheduleDortorAsync2(p =>
                (!pagedResponse.Data!.WorkDate.HasValue || (p.StartDate.HasValue && p.StartDate.Value <= conditionDate && p.EndDate.HasValue && conditionDate <= p.EndDate.Value))
                &&
                (!pagedResponse.Data!.WorkTime.HasValue || (p.StartTime.HasValue && p.StartTime.Value <= conditionTime && p.EndTime.HasValue && conditionTime <= p.EndTime.Value))
            );

            list = list.Where(p => string.IsNullOrWhiteSpace(pagedResponse.Data.NameDoctor) || StringHelper.IsMatchSearchKey(pagedResponse.Data.NameDoctor, p.Doctor!.User!.FullName)).ToList();

            var data = list.Select(x => new WorkScheduleDoctorVM()
            {
                WorkScheduleId = x.WorkScheduleId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                DoctorName = x.Doctor == null ? "" : x.Doctor!.User!.FullName
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
