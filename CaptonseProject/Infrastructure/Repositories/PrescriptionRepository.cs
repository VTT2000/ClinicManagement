using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using web_api_base.Models.ClinicManagement;

public interface IPrescriptionRepository : IRepository<Prescription>
{
    // Add custom methods for Prescription here if needed
    public Task<List<Prescription>> GetAllPrescription_PrescriptionDetail(Expression<Func<Prescription, bool>> predicate);
}

public class PrescriptionRepository : Repository<Prescription>, IPrescriptionRepository
{
    public PrescriptionRepository(ClinicContext context) : base(context)
    {
    }
    public async Task<List<Prescription>> GetAllPrescription_PrescriptionDetail(Expression<Func<Prescription, bool>> predicate)
    {
        return await _dbSet
        .Include(p => p.PrescriptionDetails)
        .Where(predicate)
        .ToListAsync();
    }
}