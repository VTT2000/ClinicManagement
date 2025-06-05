using System;
using System.Collections.Generic;

namespace web_api_base.Models.ClinicManagement;

public partial class WorkSchedule
{
    public int WorkScheduleId { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? DoctorId { get; set; }

    public virtual Doctor? Doctor { get; set; }
}
