using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using web_api_base.Models.ClinicManagement;

public interface IDiagnosisServiceRepository : IRepository<DiagnosesService>
{
    // Add custom methods for DiagnosisService here if needed
    public Task<List<DiagnosesService>> GetAllDiagnosisService_Service_User_Room(Expression<Func<DiagnosesService, bool>> predicate);
}

public class DiagnosisServiceRepository : Repository<DiagnosesService>, IDiagnosisServiceRepository
{
    public DiagnosisServiceRepository(ClinicContext context) : base(context)
    {
    }
    public async Task<List<DiagnosesService>> GetAllDiagnosisService_Service_User_Room(Expression<Func<DiagnosesService, bool>> predicate)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.Service)
            .Include(p => p.UserIdperformedNavigation)
            .Include(p=> p.Room)
            .Where(predicate)
            .ToListAsync();
    }
}