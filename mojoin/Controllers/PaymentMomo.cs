using Microsoft.AspNetCore.Mvc;
using mojoin.Models;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using mojoin.Services;
using System.Security.Cryptography;
using System.Text;
using ProGCoder_MomoAPI.Models.Order;
using Microsoft.AspNetCore.Authorization;

namespace mojoin.Controllers
{
    public class PaymentMomo : Controller
    {
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

    }
}
