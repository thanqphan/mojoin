using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class Package
{
    public int PackageId { get; set; }

    public string? PackageType { get; set; }

    public double? Price { get; set; }

    public virtual ICollection<PackageDetail> PackageDetails { get; set; } = new List<PackageDetail>();
}
