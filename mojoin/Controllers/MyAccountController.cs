using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using mojoin.Models;

namespace mojoin.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly DbmojoinContext _context;
        public INotyfService _notyfService { get; }
        public MyAccountController(DbmojoinContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
