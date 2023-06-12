using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class RoomFavorite
{
    public int FavoriteId { get; set; }

    public int RoomId { get; set; }

    public int UserId { get; set; }

    public virtual Room Room { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
