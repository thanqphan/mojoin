using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class RoomFurniture
{
    public int RoomFurnitureId { get; set; }

    public int? RoomId { get; set; }

    public int? FurniturelD { get; set; }

    public int? Quantity { get; set; }

    public virtual Furniture? FurniturelDNavigation { get; set; }

    public virtual Room? Room { get; set; }
}
