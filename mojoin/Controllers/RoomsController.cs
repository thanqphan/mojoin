using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mojoin.Models;

namespace mojoin.Controllers
{
    public class RoomsController : Controller
    {
        private readonly DbmojoinContext _context;
        
        DbmojoinContext db = new DbmojoinContext();

        public RoomsController(DbmojoinContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var dbmojoinContext = _context.Rooms.Include(r => r.Address).Include(r => r.RoomType).Include(r => r.Status).Include(r => r.User);
            return View(await dbmojoinContext.ToListAsync());
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int id)
        {
            var D_sach = _context.Rooms.Where(m => m.RoomId == id).First();
            return View(D_sach);
        }
    }
}
