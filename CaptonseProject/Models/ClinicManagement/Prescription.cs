using System;
using System.Collections.Generic;

namespace web_api_base.Models.ClinicManagement;

public partial class Prescription
{
    public int PrescriptionId { get; set; }

    public int? DiagnosisId { get; set; }

    public string? Prescription1 { get; set; }

    public virtual Diagnosis? Diagnosis { get; set; }

    public virtual ICollection<PrescriptionDetail> PrescriptionDetails { get; set; } = new List<PrescriptionDetail>();
}
