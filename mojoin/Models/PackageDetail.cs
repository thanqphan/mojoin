using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class PackageDetail
{
    public int PackageDetailId { get; set; }

    public int? PackageId { get; set; }

    public int? Duration { get; set; }

    public virtual Package? Package { get; set; }

    public virtual ICollection<UserPackage> UserPackages { get; set; } = new List<UserPackage>();
}
