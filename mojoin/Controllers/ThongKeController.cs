using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mojoin.Models;
using System.Drawing.Printing;
using System.Security.Claims;
using XAct.Library.Settings;
using XAct.Users;

namespace mojoin.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly DbmojoinContext db;
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Thongkeview(int id)
        {
            var taikhoanID = HttpContext.User.FindFirstValue("UserId");
            var dbmojoinContext = db.Rooms
                .Include(r => r.User)
                .ToList();
            return View(dbmojoinContext);
        }
    }
}
