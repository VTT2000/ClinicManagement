using web_api_base.Models.ClinicManagement;

public interface IPrescriptionRepository : IRepository<Prescription>
{
    // Add custom methods for Prescription here if needed
}

public class PrescriptionRepository : Repository<Prescription>, IPrescriptionRepository
{
    public PrescriptionRepository(ClinicContext context) : base(context)
    {
    }
}