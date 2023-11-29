using Microsoft.AspNetCore.Mvc;
using mojoin.Models;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using mojoin.Services;
using System.Security.Cryptography;
using System.Text;
using ProGCoder_MomoAPI.Models.Order;
using Microsoft.AspNetCore.Authorization;
using XAct.Library.Settings;
using XAct.Users;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace mojoin.Controllers
{
    public class PaymentMomo : Controller
    {
        DbmojoinContext db = new DbmojoinContext();
        private IMomoService _momoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PaymentMomo(IMomoService momoService, IHttpContextAccessor httpContextAccessor)
        {
            _momoService = momoService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var taikhoanID = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            if (taikhoanID == null || taikhoanID.ToString() == "")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]      
        public async Task<IActionResult> CreatePaymentUrl(TransactionHistory model)
        {
            var response = await _momoService.CreatePaymentAsync(model);
            return Redirect(response.PayUrl);
        }

        [HttpGet]
        public IActionResult PaymentCallBack()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            return View(response);
        }
        public IActionResult NaptienLayout()
        {
            var userId = HttpContext.User.FindFirstValue("UserId");
            var transactionHistory = db.TransactionHistories
                .Include(rf => rf.User)
                .Where(rf => rf.UserId == int.Parse(userId))
                .ToList();
                
            return View(transactionHistory);
        }
        public IActionResult Lichsunaptien()
        {
            var userId = HttpContext.User.FindFirstValue("UserId");
            var transactionHistory = db.TransactionHistories
                .Include(rf => rf.User)
                .Where(rf => rf.UserId == int.Parse(userId))
                .ToList();
            return View(transactionHistory);
        }
    }
}
