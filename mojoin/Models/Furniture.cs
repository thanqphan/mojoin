using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class Furniture
{
    public int FurniturelD { get; set; }

    public string? FurbitureName { get; set; }

    public virtual ICollection<RoomFurniture> RoomFurnitures { get; set; } = new List<RoomFurniture>();
}
