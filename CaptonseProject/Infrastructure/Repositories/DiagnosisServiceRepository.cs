using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using web_api_base.Models.ClinicManagement;

public interface IDiagnosisServiceRepository : IRepository<DiagnosesService>
{
    // Add custom methods for DiagnosisService here if needed
}

public class DiagnosisServiceRepository : Repository<DiagnosesService>, IDiagnosisServiceRepository
{
    public DiagnosisServiceRepository(ClinicContext context) : base(context)
    {
    }
}