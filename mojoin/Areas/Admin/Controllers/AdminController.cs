using Microsoft.AspNetCore.Mvc;

namespace mojoin.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
