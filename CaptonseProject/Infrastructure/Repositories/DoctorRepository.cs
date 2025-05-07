using Microsoft.EntityFrameworkCore;
using web_api_base.Models.ClinicManagement;

public interface IDoctorRepository : IRepository<Doctor>
{
    // Add custom methods for Doctor here if needed
    public Task<List<Doctor>> GetAllDoctorUserAsync();
}

public class DoctorRepository : Repository<Doctor>, IDoctorRepository
{
    public DoctorRepository(ClinicContext context) : base(context)
    {

    }
    public async Task<List<Doctor>> GetAllDoctorUserAsync(){
        return await _dbSet.AsNoTracking().Include(p=>p.User).ToListAsync();
    }
}