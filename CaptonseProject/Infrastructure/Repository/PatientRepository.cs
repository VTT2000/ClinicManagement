using web_api_base.Models.ClinicManagement;

public interface IPatientRepository : IRepository<Patient>
{
    // Add custom methods for Patient here if needed
}

public class PatientRepository : Repository<Patient>, IPatientRepository
{
    public PatientRepository(ClinicContext context) : base(context)
    {
    }
}