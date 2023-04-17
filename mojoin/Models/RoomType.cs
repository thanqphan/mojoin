using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class RoomType
{
    public int RoomTypeId { get; set; }

    public string? TypeName { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
