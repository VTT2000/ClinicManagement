using System;
using System.Collections.Generic;

namespace web_api_base.Models.ClinicManagement;

public partial class Diagnosis
{
    public int DiagnosisId { get; set; }

    public int? AppointmentId { get; set; }

    public string? Symptoms { get; set; }

    public string? Diagnosis1 { get; set; }

    public string? Prescription { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual ICollection<DiagnosesService> DiagnosesServices { get; set; } = new List<DiagnosesService>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
