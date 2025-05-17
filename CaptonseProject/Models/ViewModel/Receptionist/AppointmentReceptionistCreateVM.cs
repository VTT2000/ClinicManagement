public class AppointmentReceptionistCreateVM
{
    public DateOnly AppointmentDate { get; set; }
    public TimeOnly AppointmentTime { get; set; }

    public string? Status { get; set; } = StatusConstant.AppointmentStatus.Booked;

    public int? DoctorId { get; set; }
    
    public int? PatientId { get; set; }

    public string PatientFullName { get; set; } = null!;

    public DateOnly? Dob { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }
    
    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
}


    // public class FormModel
    // {
    //     [Required(ErrorMessage = "Họ tên là bắt buộc")]
    //     [StringLength(50, MinimumLength = 2, ErrorMessage = "Họ tên từ 2 đến 50 ký tự")]
    //     public string FullName { get; set; } = "";

    //     [Required(ErrorMessage = "Email là bắt buộc")]
    //     [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    //     public string Email { get; set; } = "";

    //     [Range(1, 120, ErrorMessage = "Tuổi phải từ 1 đến 120")]
    //     public int? Age { get; set; }
    // }