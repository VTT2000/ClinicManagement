using System;
using System.Collections.Generic;

namespace web_api_base.Models.ClinicManagement;

public partial class Service
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = null!;

    public decimal Price { get; set; }

    public int? ServiceParentId { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<Service> InverseServiceParent { get; set; } = new List<Service>();

    public virtual Service? ServiceParent { get; set; }
}
