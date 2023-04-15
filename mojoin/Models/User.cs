using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public bool? Sex { get; set; }

    public DateTime? Dateofbirth { get; set; }

    public int? AccountId { get; set; }

    public virtual UserAccount? Account { get; set; }

    public virtual ICollection<RoomRating> RoomRatings { get; set; } = new List<RoomRating>();

    public virtual ICollection<RoomReport> RoomReports { get; set; } = new List<RoomReport>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
