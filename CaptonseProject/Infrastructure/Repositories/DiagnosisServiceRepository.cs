using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using web_api_base.Models.ClinicManagement;

public interface IDiagnosisServiceRepository : IRepository<DiagnosesService>
{
    // Add custom methods for DiagnosisService here if needed
    public Task<List<DiagnosesService>> GetAllDiagnosisServiceAndService(Expression<Func<DiagnosesService, bool>> predicate);
}

public class DiagnosisServiceRepository : Repository<DiagnosesService>, IDiagnosisServiceRepository
{
    public DiagnosisServiceRepository(ClinicContext context) : base(context)
    {
    }
    public async Task<List<DiagnosesService>> GetAllDiagnosisServiceAndService(Expression<Func<DiagnosesService, bool>> predicate)
    {
        return await _dbSet
                .AsNoTracking()
                .Where(predicate)
                .Include(a => a.Service)
                .ToListAsync();
    }
}