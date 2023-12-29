using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mojoin.Models;
using System.Drawing.Printing;
using System.Globalization;
using System.Security.Claims;
using XAct.Library.Settings;
using XAct.Users;

namespace mojoin.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly DbmojoinContext db;
        
        public IActionResult Thongkeview()
        {
            return View();
        }
    }
}
