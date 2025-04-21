// unitofwork
using web_api_base.Models.ClinicManagement;

public interface IUnitOfWork : IAsyncDisposable
{
    public IUserRepository _userRepository{get;} //Có thì sẽ dễ quản lý
    public IPatientRepository _patientRepository{get;}
    // sẽ dễ quản lý
    Task<int> SaveChangesAsync();
}

public class UnitOfWork: IUnitOfWork
{
    public IUserRepository _userRepository{get;}
    public IPatientRepository _patientRepository{get;}

    private readonly ClinicContext _context;
    
    public UnitOfWork(ClinicContext context, IUserRepository userRepository, IPatientRepository patientRepository) 
    {
        _context = context;
        _userRepository = userRepository;
        _patientRepository = patientRepository;
    }
    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}