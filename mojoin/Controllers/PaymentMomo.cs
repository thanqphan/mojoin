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
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.Extensions.Options;
using mojoin.Models.Momo;
using RestSharp;
using static mojoin.Services.MomoService;

namespace mojoin.Controllers
{
    public class PaymentMomo : Controller
    {
        DbmojoinContext db = new DbmojoinContext();
        PaymentResponse paymentResponse;
        private readonly IOptions<MomoOptionModel> _options;
        private IMomoService _momoService;
        public INotyfService _notyfService { get; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public PaymentMomo(IOptions<MomoOptionModel> options, INotyfService notyfService, IMomoService momoService, IHttpContextAccessor httpContextAccessor)
        {
            _momoService = momoService;
            _options = options;
            _notyfService = notyfService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var taikhoanID = HttpContext.User.FindFirstValue("UserId");
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
        public async Task<IActionResult> PaymentCallBack(TransactionHistory model)
        {
            // Tạo OrderId từ Timestamp
            model.TransactionReference = DateTime.UtcNow.Ticks.ToString();
            var taikhoanID = HttpContext.User.FindFirstValue("UserId");
            //lấy thông tin trả về của momo
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);



            // Kiểm tra trạng thái thanh toán
            if (response.Message == "Success")
            {
                using (var context =  db)
                {
                    var user = await context.Users.FindAsync(int.Parse(taikhoanID));
                    var order = new TransactionHistory
                    {
                        UserId = int.Parse(taikhoanID),
                        PaymentMethod = "MoMo",
                        TransactionReference = model.TransactionReference,
                        Amount = model.Amount,
                        Note = response.OrderInfo,
                        TransactionType = "Nạp tiền",
                        TransactionDate = DateTime.Now,
                        PromotionAmount = 0,
                        ReceivedAmount = model.Amount,
                        Status = 1
                        // Các trường khác của đối tượng Order nếu cần
                    };

                    context.TransactionHistories.Add(order);
                    await context.SaveChangesAsync();
                    _notyfService.Success("Giao dịch thành công");

                    var userValue = await context.Users.FindAsync(int.Parse(taikhoanID));
                    if (userValue != null)
                    {
                        // Tính toán số tiền mới
                        userValue.Balance = userValue.Balance + model.Amount;

                        // Cập nhật lại User trong cơ sở dữ liệu
                        context.Users.Update(userValue);
                        await context.SaveChangesAsync();
                    }
                }
            }
            else
            {
                
                var order = new TransactionHistory
                {
                    UserId = int.Parse(taikhoanID),
                    PaymentMethod = "MoMo",
                    TransactionReference = model.TransactionReference,
                    Amount = model.Amount,
                    Note = response.OrderInfo,
                    TransactionType = "Nạp tiền",
                    TransactionDate = DateTime.Now,
                    PromotionAmount = 0,
                    ReceivedAmount = 0,
                    Status = 0
                };
                // Lưu vào cơ sở dữ liệu
                db.TransactionHistories.Add(order);
                db.SaveChangesAsync();
                _notyfService.Error("Giao dịch thất bại");
                return Redirect("/PaymentMomo/NaptienLayout");               
            }
            return View(response);
        }
        public IActionResult NaptienLayout()
        {
            var userId = HttpContext.User.FindFirstValue("UserId");
            var transactionHistory = db.TransactionHistories
                .Include(rf => rf.User)
                .Where(rf => rf.UserId == int.Parse(userId))
                .ToList();
            // Tính tổng số tiền từ danh sách TransactionHistories
            //double? totalAmount = transactionHistory.Where(th => th.Status == 1 ).Sum(th => th.Amount);

            //// Cập nhật Balance trong User
            //var user = db.Users.Find(int.Parse(userId));
            //if (user != null)
            //{
            //    user.Balance = totalAmount;
            //    db.SaveChanges();
            //}

            //// Gán giá trị vào ViewBag
            //ViewBag.SoTien = totalAmount;
            return View(transactionHistory);
        }
        public IActionResult Lichsunaptien()
        {
            var userId = HttpContext.User.FindFirstValue("UserId");
            var transactionHistory = db.TransactionHistories
                .Include(rf => rf.User)
                .Where(rf => rf.UserId == int.Parse(userId) && rf.TransactionType == "Nạp tiền")
                .OrderByDescending(rf => rf.TransactionDate)
                .ToList();
            return View(transactionHistory);
        }
    }
}
