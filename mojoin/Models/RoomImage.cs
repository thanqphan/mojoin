using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace mojoin.Models;

public partial class RoomImage
{
    public int RoomImageId { get; set; }

    public int RoomId { get; set; }

    public string? Image { get; set; }

    //[ForeignKey("RoomId")]
    public virtual Room Room { get; set; } = null!;
}
