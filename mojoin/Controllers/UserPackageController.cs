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
            var roomTypes = _context.RoomTypes.ToList();
            ViewBag.RoomTypes = roomTypes;
            return View();
        }
        
        
        [HttpPost]
        [Route("thanh-toan-dang-bai.html", Name = "ThanhToanTin")]
        public async Task<IActionResult> PaymentPost(UserPackageTransactionViewModel user)
        {
            var roomTypes = _context.RoomTypes.ToList();
            ViewBag.RoomTypes = roomTypes;
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
                    return View(user);
                }
                /*string videoUrl = (room.Video).ToString();
                string in_videoUrl = Regex.Replace(videoUrl, "/watch\\?v=", "/embed/");
                Room user = new Room
                {
                    RoomTypeId = room.RoomTypeId,
                    UserId = room.UserId,
                    Title = room.Title,
                    Description = room.Description,
                    Price = room.Price,
                    Area = room.Area,
                    NumRooms = room.NumRooms,
                    NumBathrooms = room.NumBathrooms,
                    CreateDate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    IsActive = 0,
                    StreetNumber = room.StreetNumber,
                    Street = room.Street,
                    Ward = room.Ward,
                    District = room.District,
                    City = room.City,
                    HasAirConditioner = room.HasAirConditioner,
                    HasElevator = room.HasElevator,
                    HasParking = room.HasParking,
                    HasRefrigerator = room.HasRefrigerator,
                    HasWasher = room.HasWasher,
                    ViewCount = 1,
                    Video = in_videoUrl,
                    DisplayType = 0,
                };*/

                _context.Add(user);
                await _context.SaveChangesAsync();
                /*int roomId = user.RoomId; // Thực hiện tạo mới roomId

                // Lưu roomId vào TempData để sử dụng trong action UploadImages
                TempData["RoomId"] = roomId;*/

                // Các logic xử lý khác của action CreatePosts
                _notyfService.Success("Gửi bài thành công!");

                /*var taikhoan = _context.Users.Find(Convert.ToInt32(user.UserId));
                var staff = _context.Users.FirstOrDefault(u => u.UserId == taikhoan.UserId);
                staff.SupportUserId;*/

                // Trả về kết quả JSON để sử dụng trong JavaScript
                return Json(new { success = true/*, roomId*/ });
            }
            catch
            {
                _notyfService.Error("Gửi bài không thành công!");
                return Json(new { success = false, message = "Đã xảy ra lỗi khi tạo bài đăng!" });
            }
        }
        
    }
}
