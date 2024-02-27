using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class RoomImagesVr
{
    public int RoomImagesVrId { get; set; }

    public int? RoomId { get; set; }

    public string? Image { get; set; }

    public virtual Room? Room { get; set; }
}
