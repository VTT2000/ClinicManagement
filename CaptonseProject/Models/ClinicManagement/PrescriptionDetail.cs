using System;
using System.Collections.Generic;

namespace web_api_base.Models.ClinicManagement;

public partial class PrescriptionDetail
{
    public int PrescriptionDetailId { get; set; }

    public int? PrescriptionId { get; set; }

    public int MedicineId { get; set; }

    public int Quantity { get; set; }

    public virtual Medicine Medicine { get; set; } = null!;

    public virtual Prescription? Prescription { get; set; }
}
