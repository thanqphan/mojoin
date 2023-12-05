using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class Package
{
    public int PackageId { get; set; }

    public string? PackageName { get; set; }

    public double? Price { get; set; }

    public int? PackageTypeId { get; set; }

    public virtual PackageType? PackageType { get; set; }

    public virtual ICollection<UserPackage> UserPackages { get; set; } = new List<UserPackage>();
}
