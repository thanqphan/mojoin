using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mojoin.Extension;
using mojoin.Models;
using mojoin.ViewModel;
using System.Data;
namespace mojoin.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {

        private readonly DbmojoinContext _context;

        public INotyfService _notyfService { get; set; }

        public HomeController(DbmojoinContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        [Area("Admin")]
        [Authorize(Policy = "AdminOrStaff")]
        public IActionResult Index()
        {
            // Lấy số lượng ghi nhận xem phòng
            int viewCount = (int)_context.Rooms.Sum(r => r.ViewCount);

            // Lấy số lượng tài khoản
            int userCount = _context.Users.Count();

            

            // Lấy doanh thu
            decimal receivedAmount = (decimal)_context.TransactionHistories.Sum(t => t.ReceivedAmount);

            // Lấy số lượng gói đã được mua
            int packageCount = _context.TransactionHistories.Count();

            // Truyền dữ liệu vào ViewBag để sử dụng trong view
            ViewBag.ViewCount = viewCount;
            ViewBag.UserCount = userCount;
            ViewBag.ReceivedAmount = receivedAmount;
            ViewBag.PackageCount = packageCount;

            return View();
        }
    }
}
