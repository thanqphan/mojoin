using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("dang-bai.html", Name = "DangBai")]
        public IActionResult CreatePosts()
        {
            var roomTypes = _context.RoomTypes.ToList();
            ViewBag.RoomTypes = roomTypes;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
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
                }
                if (ModelState.IsValid)
                {
                    //*						string salt = Utilities.GetRandomKey();
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
                    try
                    {
                        _context.Add(user);
                        await _context.SaveChangesAsync();

                        _notyfService.Success("Gửi bài thành công!");
                        return RedirectToAction("Index", "MyAccount");
                    }
                    catch
                    {
                        return RedirectToAction("Register", "Account");
                    }
                }
                else
                {               
                    _notyfService.Error("Gửi bài không thành công!");
                    return View(room);
                }
            }
            catch
            {
                return View(room);
            }
        }
    }

}
