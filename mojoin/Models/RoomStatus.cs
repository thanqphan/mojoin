using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class RoomStatus
{
    public int StatusId { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
