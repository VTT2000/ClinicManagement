using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using web_api_base.Models.ClinicManagement;

public interface IAppointmentRepository : IRepository<Appointment>
{
    // Add custom methods for Appointment here if needed
    Task<List<Appointment>> GetAllAppointmentPatientUserAsync(Expression<Func<Appointment, bool>> predicate);
    public Task<List<Appointment>> GetAllAppointmentPatientDoctorUser(Expression<Func<Appointment, bool>> predicate);
}

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ClinicContext context) : base(context)
    {

    }

    public async Task<List<Appointment>> GetAllAppointmentPatientUserAsync(Expression<Func<Appointment, bool>> predicate)
    {
        return await _dbSet
                .AsNoTracking()
                .Where(predicate)
                .Include(a => a.Patient).ThenInclude(p => p!.User) // Bệnh nhân và người dùng của bệnh nhân
                .ToListAsync();
    }
    
    public async Task<List<Appointment>> GetAllAppointmentPatientDoctorUser(Expression<Func<Appointment, bool>> predicate)
    {
        return await _dbSet
                .AsNoTracking()
                .Where(predicate)
                .Include(a => a.Patient).ThenInclude(p => p!.User) // Bệnh nhân và người dùng của bệnh nhân
                .Include(b => b.Doctor).ThenInclude(q => q!.User) // Bác sĩ và người dùng của bác sĩ
                .ToListAsync();
    }

}