using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;

using mojoin.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mojoin.Models;
using CodeMegaVNPay.Models;
using XAct.Library.Settings;

namespace mojoin.Controllers
{
    public class PaymentVnPay : Controller
    {
        DbmojoinContext db = new DbmojoinContext();
        private readonly IVnPayService _vnPayService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public INotyfService _notyfService { get; }
        public PaymentVnPay(IVnPayService vnPayService, IHttpContextAccessor httpContextAccessor, INotyfService notyfService)
        {
            _vnPayService = vnPayService;
            _httpContextAccessor = httpContextAccessor;
            _notyfService = notyfService;
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
        public IActionResult CreatePaymentUrl(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }
        [HttpGet]
        public async Task<IActionResult> PaymentCallback(TransactionHistory model)
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            
            var taikhoanID = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            //lấy thông tin trả về của momo
            // Kiểm tra trạng thái thanh toán
            if (response.VnPayResponseCode == "00")
            {
                using (var context = new DbmojoinContext())
                {
                    var user = await context.Users.FindAsync(int.Parse(taikhoanID));
                    var order = new TransactionHistory
                    {
                        UserId = int.Parse(taikhoanID),
                        PaymentMethod = "VnPay",
                        TransactionReference = response.OrderId,
                        Amount = Convert.ToDouble(response.Amount) / 100,
                        Note = response.OrderDescription,
                        TransactionType = "Nạp tiền",
                        TransactionDate = DateTime.Now,
                        PromotionAmount = 0,
                        ReceivedAmount = Convert.ToDouble(response.Amount) / 100,
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
                    return Redirect("/PaymentMomo/NaptienLayout");
                }
            }
            else
            {

                var order = new TransactionHistory
                {
                    UserId = int.Parse(taikhoanID),
                    PaymentMethod = "VnPay",
                    TransactionReference = response.OrderId,
                    Amount = Convert.ToDouble(response.Amount) / 100,
                    Note = response.OrderDescription,
                    TransactionType = "Nạp tiền",
                    TransactionDate = DateTime.Now,
                    PromotionAmount = 0,
                    ReceivedAmount = Convert.ToDouble(response.Amount) / 100,
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
    }
}
