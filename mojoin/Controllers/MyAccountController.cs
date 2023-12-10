﻿using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
    public class MyAccountController : Controller
    {
        private readonly DbmojoinContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public INotyfService _notyfService { get; }
        public MyAccountController(DbmojoinContext context, INotyfService notyfService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _notyfService = notyfService;
            _httpContextAccessor = httpContextAccessor;
        }
        [Route("quan-li-tin.html", Name = "QuanLiTin")]
        public IActionResult Index()
        {
            // Get current user's ID
            var userId = HttpContext.User.FindFirstValue("UserId");

            // Eager load related entities using Include()
            var roomFavorites = _context.Rooms
                .Include(r => r.User)
                .Include(r => r.UserPackages)
                .Include(r => r.RoomImages)
                .Where(r => r.UserId == int.Parse(userId))
                .ToList();

            // Create a collection to hold the PostManagementViewModel
            var viewModels = new List<PostManagementViewModel>();

            // Iterate through each room favorite and add it to the collection
            foreach (var roomFavorite in roomFavorites)
            {
                var viewModel = new PostManagementViewModel();
                viewModel.RoomId = roomFavorite.RoomId;
                viewModel.UserId = roomFavorite.UserId;
                viewModel.RoomTypeId = roomFavorite.RoomTypeId;
                viewModel.Title = roomFavorite.Title;
                viewModel.Price = roomFavorite.Price;
                viewModel.IsActive = roomFavorite.IsActive;
                viewModel.DisplayType = roomFavorite.DisplayType;
                viewModel.UserPackages = roomFavorite.UserPackages;
                viewModel.RoomImages = roomFavorite.RoomImages;

                viewModels.Add(viewModel);
            }

            return View(viewModels);
        }

        [HttpGet]
        [Route("doi-pass.html", Name = "DoiPassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [Route("doi-pass.html", Name = "DoiPassword")]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var taikhoanID = model.UserId.ToString();
                if (taikhoanID == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                if (ModelState.IsValid)
                {
                    var taikhoan = _context.Users.Find(Convert.ToInt32(taikhoanID));
                    if (taikhoan == null) return RedirectToAction("Login", "Account");
                    var pass = (model.PasswordNow.Trim());
                    {
                        string passnew = (model.Password.Trim());
                        taikhoan.Password = passnew;
                        _context.Update(taikhoan);
                        _context.SaveChanges();
                        _notyfService.Success("Đổi mật khẩu thành công");
                        return RedirectToAction("Login", "Account");
                    }
                }
            }
            catch
            {
                _notyfService.Error("Thay đổi không thành công");
                return View();
            }
            _notyfService.Error("Thay đổi không thành công");
            return View();
        }
        // GET: ProfileUserViewModels/Edit/5
        [HttpGet]
        [Route("update-in4.html", Name = "UpdateIn4")]
        public IActionResult UpdateUserIn4()
        {
            return View();
        }

        // POST: ProfileUserViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("update-in4.html", Name = "UpdateIn4")]
        public async Task<IActionResult> UpdateUserIn4(ProfileUserViewModel profileUserViewModel)
        {
            var taikhoanID = _httpContextAccessor.HttpContext.Session.GetString("UserId");
            if (int.Parse(taikhoanID) != profileUserViewModel.UsserId)
            {
                return NotFound();
            }
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
                return View();
            }
            if (ModelState.IsValid)
            {
                var taikhoan = _context.Users.Find(Convert.ToInt32(taikhoanID));
                try
                {
                    taikhoan.FirstName = profileUserViewModel.FirstName;
                    taikhoan.LastName = profileUserViewModel.LastName;
                    taikhoan.Address = profileUserViewModel.Address;
                    string newImagePath = TempData.Peek("NewImagePath") as string;

                    taikhoan.Avatar = newImagePath;
                    taikhoan.Email = profileUserViewModel.Email;
                    taikhoan.Sex = profileUserViewModel.Sex;
                    _context.Update(taikhoan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    /*if (!ProfileUserViewModelExists(profileUserViewModel.UsserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }*/
                }
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("UserId");
                return RedirectToAction("Login", "Account");
            }
            return View();
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
                string videoUrl = (room.Video).ToString();
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
                };

                _context.Add(user);
                await _context.SaveChangesAsync();
                int roomId = user.RoomId; // Thực hiện tạo mới roomId
                string roomTitle=user.Title;

                // Lưu roomId vào TempData để sử dụng trong action UploadImages
                TempData["RoomId"] = roomId;
                TempData["RoomTitle"]=roomTitle;

                // Các logic xử lý khác của action CreatePosts
                _notyfService.Success("Gửi bài thành công!");

                /*var taikhoan = _context.Users.Find(Convert.ToInt32(user.UserId));
                var staff = _context.Users.FirstOrDefault(u => u.UserId == taikhoan.UserId);
                staff.SupportUserId;*/

                // Trả về kết quả JSON để sử dụng trong JavaScript
                return Json(new { success = true, roomId, roomTitle });
            }
            catch
            {
                _notyfService.Error("Gửi bài không thành công!");
                return Json(new { success = false, message = "Đã xảy ra lỗi khi tạo bài đăng!" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                    string imagePath = "/images/" + fileName; // Đường dẫn mới của hình ảnh
                    TempData["NewImagePath"] = imagePath;

                    // Chuyển hướng đến action UpdateUserIn4
                    return RedirectToAction("UpdateUserIn4");
                }
            }

            // Trả về null nếu không có hình ảnh được tải lên
            return null;
        }




        [HttpPost]
        public async Task<IActionResult> UploadImages(List<IFormFile> images)
        {
            int roomId = Convert.ToInt32(TempData["RoomId"]); // Lấy ra ID của phòng từ TempData
            string roomTitle = TempData["RoomTitle"] as string;
            try
            {
                foreach (var image in images)
                {
                    // Tạo một tên mới cho ảnh bằng cách thêm mã phòng vào trước tên ảnh
                    var imageName = roomId + "_" + image.FileName;

                    // Xử lý tệp ảnh, ví dụ: lưu trữ ảnh trong thư mục "wwwroot/Images" của dự án
                    if (image.Length > 0)
                    {
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageName);
                        using (var fileStream = new FileStream(imagePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        // Lưu thông tin ảnh vào cơ sở dữ liệu
                        var imageModel = new RoomImage
                        {
                            RoomId = roomId,
                            Image = Url.Content("~/images/" + imageName)
                        };

                        // Sử dụng đối tượng DbContext để thêm bản ghi mới vào bảng ảnh
                        _context.RoomImages.Add(imageModel);
                        await _context.SaveChangesAsync();
                    }
                }
                _notyfService.Success("Gửi bài thành công!");
                TempData["SuccessMessage"] = "Đã gửi bài thành công";
                TempData["RoomTitle"] = roomTitle;
                // Hoàn thành quá trình tải lên và chuyển hướng hoặc trả về thông báo thành công
                return Json(new { success = true, roomId, roomTitle });
            }
            catch
            {
                _notyfService.Error("Upload không thành công!");
                return Json(new { success = false, message = "Đã xảy ra lỗi khi tạo bài đăng!" });
            }
        }

    }

}
