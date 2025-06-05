using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using web_api_base.Models.ClinicManagement;

public interface IWorkScheduleRepository : IRepository<WorkSchedule>
{
    // Add custom methods for WorkSchedule here if needed
    public Task<List<WorkSchedule>> GetAllWorkScheduleDortorAsync();
    public Task<List<WorkSchedule>> GetAllWorkScheduleDortorAsync2(Expression<Func<WorkSchedule, bool>> predicate);
}

public class WorkScheduleRepository : Repository<WorkSchedule>, IWorkScheduleRepository
{
    public WorkScheduleRepository(ClinicContext context) : base(context)
    {
    }
    public async Task<List<WorkSchedule>> GetAllWorkScheduleDortorAsync(){
        return await _dbSet.AsNoTracking().Include(p=>p.Doctor).ThenInclude(q=>q!.User).ToListAsync();
    }

    public async Task<List<WorkSchedule>> GetAllWorkScheduleDortorAsync2(Expression<Func<WorkSchedule, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Where(predicate).Include(p => p.Doctor).ThenInclude(q => q!.User).ToListAsync();
    }
}