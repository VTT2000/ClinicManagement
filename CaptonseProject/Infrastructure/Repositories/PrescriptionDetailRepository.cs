using web_api_base.Models.ClinicManagement;

public interface IPrescriptionDetailRepository : IRepository<PrescriptionDetail>
{
    // Add custom methods for PrescriptionDetail here if needed
}

public class PrescriptionDetailRepository : Repository<PrescriptionDetail>, IPrescriptionDetailRepository
{
    public PrescriptionDetailRepository(ClinicContext context) : base(context)
    {
    }
}