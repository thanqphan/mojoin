using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class RoomAddress
{
    public int AddressId { get; set; }

    public string? Address { get; set; }

    public string? StreetName { get; set; }

    public string? Ward { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
