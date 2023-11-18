using mojoin.Models;
using System.ComponentModel.DataAnnotations;

namespace mojoin.ViewModel
{
    public class GetRoomImagesVideoViewModel
    {
        [Key]
        public int UserId { get; set; }
        public IEnumerable<RoomImage> Images { get; set; }
        public string Video { get; set; }
    }
}
