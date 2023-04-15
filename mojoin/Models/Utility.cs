using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class Utility
{
    public int UtilityId { get; set; }

    public string? UtilityName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<RoomUtility> RoomUtilities { get; set; } = new List<RoomUtility>();
}
