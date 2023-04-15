using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class Street
{
    public int StreetId { get; set; }

    public string? StreetName { get; set; }

    public int? WardId { get; set; }

    public virtual Ward? Ward { get; set; }
}
