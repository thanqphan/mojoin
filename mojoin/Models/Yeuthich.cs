using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace mojoin.Models
{
    public class Yeuthich
    {
        [Key]
        public int FavoriteId { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public string? Title { get; set; }
        public string? StreetNumber { get; set; }

        public string? Street { get; set; }

        public string? Ward { get; set; }

        public string? District { get; set; }

        public string? City { get; set; }
        public int? NumRooms { get; set; }

        public int? NumBathrooms { get; set; }
        public double? Area { get; set; }
        public double? Price { get; set; }
        public string? Description { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}
