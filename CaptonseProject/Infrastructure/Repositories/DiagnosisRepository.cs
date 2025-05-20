using web_api_base.Models.ClinicManagement;

public interface IDiagnosisRepository : IRepository<Diagnosis>
{
    // Add custom methods for Diagnosis here if needed
}

public class DiagnosisRepository : Repository<Diagnosis>, IDiagnosisRepository
{
    public DiagnosisRepository(ClinicContext context) : base(context)
    {
        
    }
}