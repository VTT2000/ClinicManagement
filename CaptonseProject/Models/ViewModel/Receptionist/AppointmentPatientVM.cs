public class AppointmentPatientVM
{
    public int AppointmentId { get; set; }

    public int? PatientId { get; set; }

    public string PatientFullName { get; set; } = null!;

    public int? DoctorId { get; set; }

    public string DoctorFullName { get; set; } = null!;

    public DateOnly? AppointmentDate { get; set; }
    public TimeOnly? AppointmentTime { get; set; }

    public string? Status { get; set; }
    
    public DateOnly? Dob { get; set; }

    public string? Phone { get; set; }
}