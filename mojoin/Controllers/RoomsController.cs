using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mojoin.Models;
using System.Web;


namespace mojoin.Controllers
{
    public class RoomsController : Controller
    {
        
        DbmojoinContext db = new DbmojoinContext();

        // GET: Rooms
        public ActionResult Index()
        {
            var dbmojoinContext = db.Rooms.Include(r => r.RoomRatings).Include(r => r.RoomReports).Include(r => r.RoomFavorites).Include(r => r.RoomImages).Include(r => r.RoomType);
            return View(dbmojoinContext.ToList());
        }
        public ActionResult ShowImage(int id)
        {
            var roomImage =db.RoomImages
                .Where(r => r.RoomId == id)
                .Select(r => r.Image)
                .FirstOrDefault();
            if (roomImage == null)
            {
                return NotFound();
            }

            // Return the image as a file result
            return File(roomImage, "image/jpeg");
        }
        public ActionResult GetRoomImages(int id)
        {
            var roomImages = db.RoomImages
                .Where(ri => ri.RoomId == id)
                .ToList();

            return View(roomImages);
        }
        // GET: Rooms/Details/5
        public ActionResult Details(int id)
        {
            var D_sach = db.Rooms.Where(m => m.RoomId == id).First();
            return View(D_sach);
        }
    }
}
