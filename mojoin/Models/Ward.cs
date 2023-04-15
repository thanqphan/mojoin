using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class Ward
{
    public int WardId { get; set; }

    public string? WardName { get; set; }

    public int? DistrictId { get; set; }

    public virtual District? District { get; set; }

    public virtual ICollection<Street> Streets { get; set; } = new List<Street>();
}
