﻿using System;
using System.Collections.Generic;

namespace web_api_base.Models.ClinicManagement;

public partial class DiagnosesService
{
    public int DiagnosesServiceId { get; set; }

    public int DiagnosisId { get; set; }

    public int ServiceId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? ServiceResultReport { get; set; }

    public int? UserIdperformed { get; set; }

    public int? RoomId { get; set; }

    public virtual Diagnosis Diagnosis { get; set; } = null!;

    public virtual Room? Room { get; set; }

    public virtual Service Service { get; set; } = null!;

    public virtual User? UserIdperformedNavigation { get; set; }
}
