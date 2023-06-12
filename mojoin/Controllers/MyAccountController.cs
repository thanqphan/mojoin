using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mojoin.Models;
using mojoin.ViewModel;
using System.Security.Claims;
using XAct.Users;

namespace mojoin.Controllers
{
    [Authorize]
    public class MyAccountController : Controller
    {
        private readonly DbmojoinContext _context;
        public INotyfService _notyfService { get; }
        public MyAccountController(DbmojoinContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        [Route("quan-li-tin.html", Name = "QuanLiTin")]
        public IActionResult Index()
        {
            var userId = HttpContext.User.FindFirstValue("UserId");
            var roomFavorites = _context.Rooms.Include(r => r.User)
               .Where(rf => rf.UserId == int.Parse(userId)).ToList();
            ViewBag.dangdang = roomFavorites.Where(rf => rf.IsActive == 1).Count();
            ViewBag.dangcho = roomFavorites.Where(rf => rf.IsActive == 0).Count();
            ViewBag.tinloi = roomFavorites.Where(rf => rf.IsActive == 2).Count();
            return View(roomFavorites);
        }
        public ActionResult IndexPartial()
        {
            var userId = HttpContext.User.FindFirstValue("UserId");
            var roomFavorites = _context.Rooms.Include(r => r.User)
               .Where(rf => rf.UserId == int.Parse(userId))
               .ToList();
            ViewBag.tongsoluong = roomFavorites.Count();
            return View(roomFavorites);
        }
        public ActionResult IndexPartial0()
        {
            var userId = HttpContext.User.FindFirstValue("UserId");
            var roomFavorites = _context.Rooms.Include(r => r.User)
               .Where(rf => rf.UserId == int.Parse(userId))
               .ToList();
            ViewBag.tongsoluong = roomFavorites.Count();
            return View(roomFavorites);
        }
        public ActionResult IndexPartial2()
        {
            var userId = HttpContext.User.FindFirstValue("UserId");
            var roomFavorites = _context.Rooms.Include(r => r.User)
               .Where(rf => rf.UserId == int.Parse(userId))
               .ToList();
            ViewBag.tongsoluong = roomFavorites.Count();
            return View(roomFavorites);
        }
        [HttpGet]
        [Route("dang-bai.html", Name = "DangBai")]
        public IActionResult CreatePosts()
        {
            var roomTypes = _context.RoomTypes.ToList();
            ViewBag.RoomTypes = roomTypes;
            return View();
        }
        [HttpPost]
        [Route("dang-bai.html", Name = "DangBai")]
        public async Task<IActionResult> CreatePosts(RoomPostViewModel room)
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
                    return View(room);
                }

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
                };

                _context.Add(user);
                await _context.SaveChangesAsync();
                int roomId = user.RoomId; // Thực hiện tạo mới roomId

                // Lưu roomId vào TempData để sử dụng trong action UploadImages
                TempData["RoomId"] = roomId;

                // Các logic xử lý khác của action CreatePosts
                _notyfService.Success("Gửi bài thành công!");
                // Chuyển hướng đến action UploadImages và truyền roomId qua route data
                return RedirectToAction("Index", "MyAccount");
            }
            catch
            {
                _notyfService.Error("Gửi bài không thành công!");
                return View(room);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadImages(List<IFormFile> images)
        {
            int roomId = Convert.ToInt32(TempData["RoomId"]);            // Lấy ra ID của phòng từ TempData
            try
            {
                foreach (var image in images)
                {
                    // Xử lý tệp ảnh, ví dụ: lưu trữ ảnh trong thư mục "wwwroot/Images" của dự án
                    if (image.Length > 0)
                    {
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", image.FileName);
                        using (var fileStream = new FileStream(imagePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        // Lưu thông tin ảnh vào cơ sở dữ liệu
                        var imageModel = new RoomImage
                        {
                            RoomId = roomId,
                            Image = Url.Content("~/images/" + image.FileName)
                        };

                        // Sử dụng đối tượng DbContext để thêm bản ghi mới vào bảng ảnh
                        _context.RoomImages.Add(imageModel);
                        await _context.SaveChangesAsync();
                    }
                }
                _notyfService.Success("Gửi bài thành công!");

                // Hoàn thành quá trình tải lên và chuyển hướng hoặc trả về thông báo thành công
                return RedirectToAction("Index", "MyAccount");
            }
            catch
            {
                _notyfService.Error("Upload không thành công!");
                return RedirectToAction("CreatePosts");
            }
        }

    }

}
