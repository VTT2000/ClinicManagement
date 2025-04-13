using System;
using System.Collections.Generic;

namespace web_api_base.Models.dbebay;

public partial class GetListOrderDetailByOrderId
{
    public int Id { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? OrderDetail { get; set; }
}
