using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class UserPackage
{
    public int UserPackageId { get; set; }

    public int? UserId { get; set; }

    public int? PackageId { get; set; }

    public int? RoomId { get; set; }

    public int? TransactionId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? Duration { get; set; }

    public virtual Package? Package { get; set; }

    public virtual Room? Room { get; set; }

    public virtual TransactionHistory? Transaction { get; set; }

    public virtual User? User { get; set; }
}
