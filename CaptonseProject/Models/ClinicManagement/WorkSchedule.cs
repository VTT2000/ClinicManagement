using System;
using System.Collections.Generic;

namespace web_api_base.Models.ClinicManagement;

public partial class WorkSchedule
{
    public int WorkScheduleId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? DayOfWeek { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
