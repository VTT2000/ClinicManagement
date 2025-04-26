using web_api_base.Models.ClinicManagement;

public interface IDoctorRepository : IRepository<Doctor>
{
    // Add custom methods for Doctor here if needed
}

public class DoctorRepository : Repository<Doctor>, IDoctorRepository
{
    public DoctorRepository(ClinicContext context) : base(context)
    {
    }
}