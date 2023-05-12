using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace mojoin.Models
{
    public class Yeuthich
    {
        DbmojoinContext db = new DbmojoinContext();
        public int RoomID { get; set; }
        public string? Title { get; set; }
        public string? StreetNumber { get; set; }

        public string? Street { get; set; }

        public string? Ward { get; set; }

        public string? District { get; set; }

        public string? City { get; set; }
        public int? NumRooms { get; set; }

        public int? NumBathrooms { get; set; }
        public double? Area { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Ngày đăng")]
        public DateTime? CreateDate { get; set; }

        public Yeuthich(int id)
        {
            RoomID = id;
            Room room = db.Rooms.Single(n => (n.RoomId == RoomID));
            Title = room.Title;
            StreetNumber = room.StreetNumber;
            Street = room.Street;
            Ward = room.Ward;
            District = room.District;
            City = room.City;
            NumRooms = room.NumRooms;
            NumBathrooms = room.NumBathrooms;
            Area = room.Area;
            Description = room.Description;
            CreateDate = room.CreateDate;
        }
    }
}
