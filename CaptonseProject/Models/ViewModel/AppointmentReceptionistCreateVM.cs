public class AppointmentReceptionistCreateVM
{
    public DateTime AppointmentDate { get; set; }

    public string? Status { get; set; }

    public int? DoctorId { get; set; }
    
    public int? PatientId { get; set; }

    public string PatientFullName { get; set; } = null!;

    public DateOnly? Dob { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }
    
    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
}