namespace web_api_base.Models.ViewModel.Receptionist;
public class PatientSearchedForCreateAppointmentVM
{
    public int? PatientId { get; set; }

    public string FullName { get; set; } = null!;
    
    public DateOnly? Dob { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
}