using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using web_api_base.Models.ClinicManagement;

public interface IDoctorRepository : IRepository<Doctor>
{
    // Add custom methods for Doctor here if needed
    public Task<List<Doctor>> GetAllDoctorUserAsync();
    public Task<List<Doctor>> GetAllDoctorUserAsync(Expression<Func<Doctor, bool>> predicate);
    public Task<Doctor?> GetDoctorUserAsync(int doctorID);
}

public class DoctorRepository : Repository<Doctor>, IDoctorRepository
{
    public DoctorRepository(ClinicContext context) : base(context)
    {

    }
    public async Task<List<Doctor>> GetAllDoctorUserAsync(){
        return await _dbSet.AsNoTracking().Include(p=>p.User).ToListAsync();
    }

    public async Task<List<Doctor>> GetAllDoctorUserAsync(Expression<Func<Doctor, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Include(p=>p.User).Where(predicate).ToListAsync();
    }

    public async Task<Doctor?> GetDoctorUserAsync(int doctorID)
    {
        return await _dbSet.AsNoTracking().Include(p => p.User).FirstOrDefaultAsync(x => x.DoctorId == doctorID);
    }
}