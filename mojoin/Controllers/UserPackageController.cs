using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using mojoin.Models;
using mojoin.ViewModel;
using System.Text.RegularExpressions;

namespace mojoin.Controllers
{
    [Authorize]
    public class UserPackageController : Controller
    {
        private readonly DbmojoinContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public INotyfService _notyfService { get; }
        public UserPackageController(DbmojoinContext context, INotyfService notyfService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _notyfService = notyfService;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        [Route("thanh-toan-dang-bai.html", Name = "ThanhToanTin")]
        public IActionResult PaymentPost()
        {
            var packageTypes = _context.Packages.ToList();
            ViewBag.PackageTypes = packageTypes;
            string roomTitle = TempData["RoomTitle"] as string;
            ViewBag.RoomTitle = roomTitle;
            return View();
        }
        
        
        [HttpPost]
        [Route("thanh-toan-dang-bai.html", Name = "ThanhToanTin")]
        public async Task<IActionResult> PaymentPost(UserPackageTransactionViewModel user)
        {
            var packageTypes = _context.PackageTypes.ToList();
            ViewBag.PackageTypes = packageTypes;
            return View();
        }
        
    }
}
