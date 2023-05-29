using Microsoft.AspNetCore.Mvc;

namespace mojoin.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
