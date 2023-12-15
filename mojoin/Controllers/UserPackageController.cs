using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using mojoin.Extension;
using mojoin.Models;
using mojoin.ViewModel;
using System.Security.Claims;
using System.Text.RegularExpressions;
using XAct;
using XAct.Library.Settings;
using XAct.Users;

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
            var userIdClaim = HttpContext.User.FindFirst("UserId");
            string userId = userIdClaim.Value;
            var packageTypes = _context.Packages.ToList();
            ViewBag.PackageTypes = packageTypes;
            string roomTitle = TempData["RoomTitle"] as string;
            ViewBag.RoomTitle = roomTitle;
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId.ToInt32());
            ViewBag.UserBalance = user.Balance;
            int roomId = Convert.ToInt32(TempData["RoomId"]);
            ViewBag.RoomId = roomId;
            return View();
        }


        [HttpPost]
        [Route("thanh-toan-dang-bai.html", Name = "ThanhToanTin")]
        public async Task<IActionResult> PaymentPost(UserPackageTransactionViewModel userPackageTrans)
        {
            var userIdClaim = HttpContext.User.FindFirst("UserId");
            string userId = userIdClaim.Value;
            var packageTypes = _context.Packages.ToList();
            ViewBag.PackageTypes = packageTypes;
            string roomTitle = TempData["RoomTitle"] as string;
            ViewBag.RoomTitle = roomTitle;
            var us = _context.Users.FirstOrDefault(u => u.UserId == userId.ToInt32());
            ViewBag.UserBalance = us.Balance;
            int roomId = Convert.ToInt32(TempData["RoomId"]);
            ViewBag.RoomId = roomId;
            try
            {
                if (!ModelState.IsValid)
                {
                    foreach (var key in ModelState.Keys)
                    {
                        if (ModelState.TryGetValue(key, out ModelStateEntry entry) && entry.Errors.Any())
                        {
                            // Trường có tên là "key" có lỗi validation
                            var errorMessage = entry.Errors[0].ErrorMessage;
                            // Xử lý lỗi ở đây
                        }
                        else
                        {
                            // Trường có tên là "key" không có lỗi validation
                        }
                    }

                    _notyfService.Error("Gửi bài không thành công!");
                    return View(userPackageTrans);
                }
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        TransactionHistory userTrans = new TransactionHistory
                        {
                            UserId = userId.ToInt32(),
                            TransactionType = "Mua gói tin",
                            Amount = userPackageTrans.Amount,
                            TransactionDate = DateTime.Now,
                            PaymentMethod = "Trừ số dư",
                            ReceivedAmount = userPackageTrans.Amount,
                            Note = "Thanh toán từ số dư tài khoản thành công",
                            Status = 1,
                        };

                        _context.Add(userTrans);
                        await _context.SaveChangesAsync(); // Lưu thông tin transaction

                        int newTransId = userTrans.TransactionId;

                        UserPackage userPackage = new UserPackage
                        {
                            UserId = userId.ToInt32(),
                            PackageId = userPackageTrans.PackageID,
                            RoomId = userPackageTrans.RoomID,
                            TransactionId = newTransId,
                            StartDate = DateTime.Now,
                            EndDate = userPackageTrans.EndDate,
                            Duration = userPackageTrans.Duration,
                        };

                        _context.Add(userPackage);
                        await _context.SaveChangesAsync(); // Lưu thông tin gói tin

                        var package = _context.Packages.Where(p => p.PackageId == userPackageTrans.PackageID).FirstOrDefault();
                        int? vipType = package.Viptype;
                        var roomPost = _context.Rooms.Find(Convert.ToInt32(userPackageTrans.RoomID));
                        {
                            roomPost.IsActive = 4;
                            roomPost.DisplayType = vipType;
                            _context.Update(roomPost);
                            _context.SaveChanges();
                        }
                        var userPost = _context.Users.Find(userId.ToInt32());
                        {
                            double? userBalance = userPost.Balance;
                            double? userBalanceNow = userBalance - userPackageTrans.Amount;
                            userPost.Balance = userBalanceNow;
                            _context.Update(userPost);
                            _context.SaveChanges();
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        // Xử lý nếu có lỗi
                        transaction.Rollback();
                    }
                }
                return Json(new { success = true, roomId });
            }
            catch
            {
                _notyfService.Error("Gửi bài không thành công!");
                return Json(new { success = false, message = "Đã xảy ra lỗi khi tạo bài đăng!" });
            }
        }
        [Route("lich-su-mua-goi.html", Name = "LichSuMuaGoi")]
        public IActionResult PurchaseHistory()
        {
            var userId = HttpContext.User.FindFirstValue("UserId");
            var transactionHistory = _context.TransactionHistories
                .Include(rf => rf.User)
                .Include(rf => rf.UserPackages)
                    .ThenInclude(up => up.Package)  // Bao gồm thông tin từ bảng Package
                .Where(rf => rf.UserId == int.Parse(userId) && rf.TransactionType == "Mua gói tin")
                .OrderByDescending(rf => rf.TransactionDate)
                .ToList();
            return View(transactionHistory);
        }
    }
}
