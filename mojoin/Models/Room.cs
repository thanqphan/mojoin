using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace mojoin.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public int RoomTypeId { get; set; }

    public int UserId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public double? Price { get; set; }

    public double? Area { get; set; }

    public int? NumRooms { get; set; }

    public int? NumBathrooms { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? LastUpdate { get; set; }

    public int? IsActive { get; set; }

    public string? StreetNumber { get; set; }

    public string? Street { get; set; }

    public string? Ward { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }

    public bool HasRefrigerator { get; set; }

    public bool HasAirConditioner { get; set; }

    public bool HasWasher { get; set; }

    public bool HasElevator { get; set; }

    public bool HasParking { get; set; }

    public int? ViewCount { get; set; }

    public virtual ICollection<RoomFavorite> RoomFavorites { get; set; } = new List<RoomFavorite>();

    public virtual ICollection<RoomImage> RoomImages { get; set; } = new List<RoomImage>();

    public virtual ICollection<RoomRating> RoomRatings { get; set; } = new List<RoomRating>();

    public virtual ICollection<RoomReport> RoomReports { get; set; } = new List<RoomReport>();

    [ValidateNever]
    public virtual RoomType RoomType { get; set; } = null!;


    [ValidateNever]
    public virtual User User { get; set; } = null!;
}
