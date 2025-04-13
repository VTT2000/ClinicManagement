using System;
using System.Collections.Generic;

namespace web_api_base.Models.ClinicManagement;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int? PatientId { get; set; }

    public int? DoctorId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient? Patient { get; set; }
}
