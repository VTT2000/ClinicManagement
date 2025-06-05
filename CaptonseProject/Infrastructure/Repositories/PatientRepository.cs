using Microsoft.EntityFrameworkCore;
using web_api_base.Models.ClinicManagement;

public interface IPatientRepository : IRepository<Patient>
{

}

public class PatientRepository : Repository<Patient>, IPatientRepository
{
    private readonly ClinicContext _context;

    public PatientRepository(ClinicContext context) : base(context)
    {
        _context = context;
    }

    

}