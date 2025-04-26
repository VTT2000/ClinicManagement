using Microsoft.EntityFrameworkCore;
using web_api_base.Models.ClinicManagement;

public interface IAppointmentRepository : IRepository<Appointment>
{
    // Add custom methods for Appointment here if needed
    Task<List<Appointment>> GetAllAppointmentForReceptionistAsync(DateTime? date = null);
}

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ClinicContext context) : base(context)
    {

    }

    public async Task<List<Appointment>> GetAllAppointmentForReceptionistAsync(DateTime? date = null)
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
                .Where(c=>c.AppointmentDate.Date.Equals(date.Value.Date))
                .ToListAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}