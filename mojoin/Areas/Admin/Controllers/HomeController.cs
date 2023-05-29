using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mojoin.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        [Authorize(Policy = "AdminOrStaff")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
