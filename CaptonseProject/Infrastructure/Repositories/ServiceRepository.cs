using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using web_api_base.Models.ClinicManagement;

public interface IServiceRepository : IRepository<Service>
{
    // Add custom methods for Service here if needed
    public Task<List<Service>> GetAllService_Service(Expression<Func<Service, bool>> predicate);
}

public class ServiceRepository : Repository<Service>, IServiceRepository
{
    public ServiceRepository(ClinicContext context) : base(context)
    {
    }
    public async Task<List<Service>> GetAllService_Service(Expression<Func<Service, bool>> predicate)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(p => p.ServiceParent)
            .Where(predicate)
            .ToListAsync();
    }
}