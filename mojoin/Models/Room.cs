using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public double? Price { get; set; }

    public double? Area { get; set; }

    public int? NumRooms { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? LastUpdate { get; set; }

    public string? Image1 { get; set; }

    public string? Image2 { get; set; }

    public string? Image3 { get; set; }

    public string? Image4 { get; set; }

    public int? RoomTypeId { get; set; }

    public int? UserId { get; set; }

    public int? AddressId { get; set; }

    public int? StatusId { get; set; }

    public virtual RoomAddress? Address { get; set; }

    public virtual ICollection<RoomFurniture> RoomFurnitures { get; set; } = new List<RoomFurniture>();

    public virtual ICollection<RoomRating> RoomRatings { get; set; } = new List<RoomRating>();

    public virtual ICollection<RoomReport> RoomReports { get; set; } = new List<RoomReport>();

    public virtual RoomType? RoomType { get; set; }

    public virtual ICollection<RoomUtility> RoomUtilities { get; set; } = new List<RoomUtility>();

    public virtual RoomStatus? Status { get; set; }

    public virtual User? User { get; set; }
}
