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
            var dbmojoinContext = db.Rooms.Include(r => r.RoomType);
            return View(dbmojoinContext.ToList());
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int id)
        {
            var D_sach = db.Rooms.Where(m => m.RoomId == id).First();
            return View(D_sach);
        }
    }
}
