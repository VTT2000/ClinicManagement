using System;
using System.Collections.Generic;

namespace web_api_base.Models.ClinicManagement;

public partial class Room
{
    public int RoomId { get; set; }

    public string RoomName { get; set; } = null!;

    public virtual ICollection<DiagnosesService> DiagnosesServices { get; set; } = new List<DiagnosesService>();
}
