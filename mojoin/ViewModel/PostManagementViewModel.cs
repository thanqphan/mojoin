using mojoin.Models;
using mojoin.Services;
using System.ComponentModel.DataAnnotations;
using XAct;

namespace mojoin.ViewModel
{
    public class PostManagementViewModel
    {
        [Key]
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public int RoomTypeId { get; set; }
        public string? Title { get; set; }
        public double? Price { get; set; }
        public int? IsActive { get; set; }
        public int? DisplayType { get; set; }
        public string? StreetNumber { get; set; }

        public string? Street { get; set; }

        public string? Ward { get; set; }

        public string? District { get; set; }

        public string? City { get; set; }
        public int? ViewCount { get; set; }
        public DateTime? LastUpdate { get; set; }
        public ICollection<UserPackage> UserPackages { get; set; }
        public ICollection<RoomImage> RoomImages { get; set; }
    }
}
