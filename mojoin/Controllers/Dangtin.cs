using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mojoin.Controllers
{
    
    public class Dangtin : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
