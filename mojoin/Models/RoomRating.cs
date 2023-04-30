using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class RoomRating
{
    public int RatingId { get; set; }

    public int RoomId { get; set; }

    public int? UserId { get; set; }

    public int? RatingPoint { get; set; }

    public string? RatingComment { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Room Room { get; set; } = null!;

    public virtual User? User { get; set; }
}
