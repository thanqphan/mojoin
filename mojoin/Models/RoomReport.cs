using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class RoomReport
{
    public int ReportId { get; set; }

    public int? RoomId { get; set; }

    public int? UserId { get; set; }

    public string? ReportContent { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Room? Room { get; set; }

    public virtual User? User { get; set; }
}
