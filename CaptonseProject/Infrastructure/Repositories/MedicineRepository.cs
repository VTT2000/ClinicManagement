using web_api_base.Models.ClinicManagement;

public interface IMedicineRepository : IRepository<Medicine>
{
    // Add custom methods for Medicine here if needed
}

public class MedicineRepository : Repository<Medicine>, IMedicineRepository
{
    public MedicineRepository(ClinicContext context) : base(context)
    {
    }
}