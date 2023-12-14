using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class PackageType
{
    public int PackageTypeId { get; set; }

    public string? PackageTypeName { get; set; }

    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();
}
