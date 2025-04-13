using System;
using System.Collections.Generic;

namespace web_api_base.Models.ClinicManagement;

public partial class Patient
{
    public int PatientId { get; set; }

    public int? UserId { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual User? User { get; set; }
}
