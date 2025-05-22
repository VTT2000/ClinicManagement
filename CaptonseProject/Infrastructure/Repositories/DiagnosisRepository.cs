using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using web_api_base.Models.ClinicManagement;

public interface IDiagnosisRepository : IRepository<Diagnosis>
{
    // Add custom methods for Diagnosis here if needed
    public Task<List<Diagnosis>> GetAllDiagnosis_Appointment_DiagnosisService_Service_Room(Expression<Func<Diagnosis, bool>> predicate);
}

public class DiagnosisRepository : Repository<Diagnosis>, IDiagnosisRepository
{
    public DiagnosisRepository(ClinicContext context) : base(context)
    {

    }

    public async Task<List<Diagnosis>> GetAllDiagnosis_Appointment_DiagnosisService_Service_Room(Expression<Func<Diagnosis, bool>> predicate)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p=>p.Appointment)
            .Include(p => p.DiagnosesServices).ThenInclude(q => q.Service)
            .Include(p => p.DiagnosesServices).ThenInclude(q => q.Room)
            .Where(predicate)
            .ToListAsync();
    }
}