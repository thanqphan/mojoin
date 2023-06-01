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
    public class YeuThich : Controller
    {
        DbmojoinContext db = new DbmojoinContext();
        public IActionResult Index()
        {
            return View();
        }
    }
}
