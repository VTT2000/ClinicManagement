using web_api_base.Models.ClinicManagement;
public interface IUnitOfWork : IAsyncDisposable
{
  public IUserRepository _userRepository { get; }
  public IPatientRepository _patientRepository { get; }
  public IDoctorRepository _doctorRepository { get; }
  public IAppointmentRepository _appointmentRepository { get; }

  Task<int> SaveChangesAsync();

  // Transaction support (optional but recommended)
  Task BeginTransaction();
  Task CommitTransaction();
  Task RollBack();

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    ValueTask DisposeAsync();
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
}

public class UnitOfWork : IUnitOfWork
{
  public IUserRepository _userRepository { get; }
  public IPatientRepository _patientRepository { get; }
  public IDoctorRepository _doctorRepository { get; }
  public IAppointmentRepository _appointmentRepository { get; }

  private readonly ClinicContext _context;


  public UnitOfWork(ClinicContext context, IUserRepository userRepository, IAppointmentRepository appointmentRepository, IPatientRepository patientRepository, IDoctorRepository doctorRepository)
  {
    _context = context;
    _userRepository = userRepository;
    _patientRepository = patientRepository;
    _doctorRepository = doctorRepository;
    _appointmentRepository = appointmentRepository;
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




