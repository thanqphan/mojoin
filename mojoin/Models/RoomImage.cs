using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class RoomImage
{

    public int RoomImageId { get; set; }

    public int RoomId { get; set; }

    public string? Image { get; set; }

    public virtual Room Room { get; set; } = null!;
}
