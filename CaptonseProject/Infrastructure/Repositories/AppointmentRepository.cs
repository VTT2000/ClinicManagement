using Microsoft.EntityFrameworkCore;
using web_api_base.Models.ClinicManagement;

public interface IAppointmentRepository : IRepository<Appointment>
{
    // Add custom methods for Appointment here if needed
    Task<List<Appointment>> GetAllAppointmentForReceptionistAsync(DateOnly? date = null);
    Task<List<Appointment>> GetAllAppointmentPatientUserAsync(DateOnly date);
}

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ClinicContext context) : base(context)
    {

    }

    public async Task<List<Appointment>> GetAllAppointmentPatientUserAsync(DateOnly date){
        return await _dbSet
                .AsNoTracking()
                .Include(a => a.Patient).ThenInclude(p => p!.User) // Bệnh nhân và người dùng của bệnh nhân
                .Where(x=> date.CompareTo(x.AppointmentDate ?? DateOnly.MinValue) == 0)
                .ToListAsync();
    }

    public async Task<List<Appointment>> GetAllAppointmentForReceptionistAsync(DateOnly? date = null)
    {
        if (date == null){
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return await _dbSet
                .AsNoTracking()
                .Include(a => a.Patient).ThenInclude(p => p!.User) // Bệnh nhân và người dùng của bệnh nhân
                .Include(b => b.Doctor).ThenInclude(q => q.User) // Bác sĩ và người dùng của bác sĩ
                .ToListAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
        else{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return await _dbSet
                .AsNoTracking()
                .Include(a => a.Patient).ThenInclude(p => p!.User) // Bệnh nhân và người dùng của bệnh nhân
                .Include(b => b.Doctor).ThenInclude(q => q.User) // Bác sĩ và người dùng của bác sĩ
                .Where(c=> date.Value.CompareTo(c.AppointmentDate ?? DateOnly.MinValue) == 0)
                .ToListAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}