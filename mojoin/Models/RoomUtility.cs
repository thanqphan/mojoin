using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class RoomUtility
{
    public int RoomUtilities { get; set; }

    public int? RoomId { get; set; }

    public int? UtilityId { get; set; }

    public virtual Room? Room { get; set; }

    public virtual Utility? Utility { get; set; }
}
