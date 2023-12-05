using mojoin.Models;

namespace mojoin.Services
{
    public class RoomService
    {
        public DbmojoinContext _context { get; set; }

        public RoomService(DbmojoinContext context)
        {
            _context = context;
        }

        public Room GetRoomById(int roomId)
        {
            return _context.Rooms.FirstOrDefault(x => x.RoomId == roomId);
        }

        public ICollection<RoomImage> GetRoomImagesById(int roomId)
        {
            return _context.RoomImages.Where(x => x.RoomId == roomId).ToList();
        }

        public ICollection<UserPackage> GetUserPackagesById(int roomId)
        {
            return _context.UserPackages.Where(x => x.RoomId == roomId).ToList();
        }
    }
}
