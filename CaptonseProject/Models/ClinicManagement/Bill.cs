using System;
using System.Collections.Generic;

namespace web_api_base.Models.ClinicManagement;

public partial class Bill
{
    public int BillId { get; set; }

    public int? AppointmentId { get; set; }

    public decimal TotalAmount { get; set; }

    public string? PaymentStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Appointment? Appointment { get; set; }
}
