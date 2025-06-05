using System;
using System.Collections.Generic;

namespace web_api_base.Models.ClinicManagement;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<DiagnosesService> DiagnosesServices { get; set; } = new List<DiagnosesService>();

    public virtual Doctor? Doctor { get; set; }

    public virtual Patient? Patient { get; set; }
}
