using System;
using System.Collections.Generic;

namespace web_api_base.Models.ClinicManagement;

public partial class Prescription
{
    public int PrescriptionId { get; set; }

    public int? DiagnosisId { get; set; }

    public int? MedicineId { get; set; }

    public int Quantity { get; set; }

    public virtual Diagnosis? Diagnosis { get; set; }

    public virtual Medicine? Medicine { get; set; }
}
