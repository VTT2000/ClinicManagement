using Microsoft.EntityFrameworkCore;
using web_api_base.Models.ClinicManagement;

public interface IPatientRepository : IRepository<Patient>
{
    // Add custom methods for Patient here if needed
    public Task<List<Patient>> GetAllPatientUserAsync();
}

public class PatientRepository : Repository<Patient>, IPatientRepository
{
    public PatientRepository(ClinicContext context) : base(context)
    {

    }
    public async Task<List<Patient>> GetAllPatientUserAsync(){
        return await _dbSet.AsNoTracking().Include(p=>p.User).ToListAsync();
    }
}