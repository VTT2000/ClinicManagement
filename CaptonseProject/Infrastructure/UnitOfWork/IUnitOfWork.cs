using web_api_base.Models.ClinicManagement;
public interface IUnitOfWork : IAsyncDisposable
{
  public IUserRepository _userRepository { get; }
  public IPatientRepository _patientRepository { get; }
  Task<int> SaveChangesAsync();
}

public class UnitOfWork : IUnitOfWork
{



  public IUserRepository _userRepository { get; }

  public IPatientRepository _patientRepository { get; }

  private readonly ClinicContext _context;


  public UnitOfWork(ClinicContext context, IUserRepository userRepository)
  {
    _context = context;
    _userRepository = userRepository;
  }
  public async Task BeginTransaction()
  {
    await _context.Database.BeginTransactionAsync();
  }
  public async Task CommitTransaction()
  {
    await _context.Database.CommitTransactionAsync();
  }
  public async Task RollBack()
  {
    await _context.Database.RollbackTransactionAsync();
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




