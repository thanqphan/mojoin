using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace mojoin.ViewModel
{
    public class RoomPostViewModel
    {
        [Key]
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
    }
}
