using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mojoin.Extension;
using mojoin.Models;
using mojoin.ViewModel;
using System.Data;
using System.Security.Claims;
using XAct.Library.Settings;
using XAct.Users;
using System.Web;


namespace mojoin.Areas.Admin.Controllers
{
    [Area("Admin")]
    /*[Authorize(Roles = "Staff,Admin", Policy = "StaffOnly")]*/
    public class RoomsController : Controller
    {
        private readonly DbmojoinContext _context;
        public INotyfService _notyfService { get; }
        public RoomsController(DbmojoinContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/Rooms
        /*public async Task<IActionResult> Index()
        {
            var dbmojoinContext = _context.Rooms.Include(r => r.RoomType);
            return View(await dbmojoinContext.ToListAsync());

        }*/

        public async Task<IActionResult> Index(int isActive = -1)
        {
            HttpContext.Session.SetInt32("RoomsParams", isActive);
            ViewBag.RoomsParams = isActive;
            var dbmojoinContext = GetListRoomByActive(isActive);
            ViewData["QuyenAcc"] = new SelectList(_context.Roles, "RoleName", "RoleName");
            List<SelectListItem> lsTrangThai = new List<SelectListItem>();
            List<SelectListItem> lsLoaiPhong = new List<SelectListItem>();
            lsTrangThai.Add(new SelectListItem() { Text = "Đã duyệt", Value = "1" });
            lsTrangThai.Add(new SelectListItem() { Text = "Không được duyệt", Value = "2" });
            lsTrangThai.Add(new SelectListItem() { Text = "Bài đăng đang chờ xử lý", Value = "0" });

            lsLoaiPhong.Add(new SelectListItem() { Text = "Nhà trọ", Value = "1" });
            lsLoaiPhong.Add(new SelectListItem() { Text = "Căn hộ", Value = "2" });
            lsLoaiPhong.Add(new SelectListItem() { Text = "Ở chung chủ", Value = "3" });
            lsLoaiPhong.Add(new SelectListItem() { Text = "Ở ghép", Value = "4" });
            ViewData["lsLoaiPhong"] = lsLoaiPhong;
            ViewData["lsTrangThai"] = lsTrangThai;
            //
            return View(await dbmojoinContext.ToListAsync());
        }

        public IQueryable<Room> GetListRoomByActive(int isActive)
        {
            ViewBag.RoomsParams = isActive;
            return isActive == -1 ? _context.Rooms.Include(r => r.RoomType) :
                _context.Rooms.Include(r => r.RoomType).Where(x => x.IsActive == isActive);
        }

        public IActionResult ListRoom(int isActive = -1)
        {
            var dbmojoinContext = GetListRoomByActive(isActive);
            return PartialView(dbmojoinContext.ToListAsync());
        }

        // GET: Admin/Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }
            var roomImages = await _context.RoomImages
            .Where(ri => ri.RoomId == id)
            .ToListAsync();
            ViewBag.RoomImages = roomImages;
            var us = room.UserId;
            var userName = await _context.Users.FirstOrDefaultAsync(u => u.UserId == us);
            ViewData["UserName"] = new SelectList(_context.Users, "UserId", "FirstName");
            
            ViewBag.RoomsParams = HttpContext.Session.GetInt32("RoomsParams");
            return View(room);
        }

        // GET: Admin/Rooms/Create
        public IActionResult Create()
        {
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "TypeName");
            return View();
        }

        // POST: Admin/Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,RoomTypeId,Title,Description,Price,Area,NumRooms,NumBathrooms,CreateDate,LastUpdate,IsActive,StreetNumber,Street,Ward,District,City,HasRefrigerator,HasAirConditioner,HasWasher,HasElevator,HasParking,ViewCount,Files")] RoomPostViewModel room)
        {
            if (ModelState.IsValid)
            {
                var roomEntity = new Room
                {
                    RoomTypeId = room.RoomTypeId,
                    /*RoomType = roomType.TypeName,*/
                    Title = room.Title,
                    Description = room.Description,
                    Price = room.Price,
                    Area = room.Area,
                    NumRooms = room.NumRooms,
                    NumBathrooms = room.NumBathrooms,
                    IsActive = 0, // mặc định khởi tạo mới = 0
                    StreetNumber = room.StreetNumber,
                    Street = room.Street,
                    Ward = room.Ward,
                    District = room.District,
                    City = room.City,
                    HasRefrigerator = room.HasRefrigerator,
                    HasAirConditioner = room.HasAirConditioner,
                    HasWasher = room.HasWasher,
                    HasElevator = room.HasElevator,
                    HasParking = room.HasParking,
                    ViewCount = room.ViewCount,
                    UserId = HttpContext.GetUserId(),
                    CreateDate = DateTime.Now
                };
                /*var roomType = _context.RoomTypes.Find(room.RoomTypeId);*/
                _context.Rooms.Add(roomEntity);
                await _context.SaveChangesAsync();
                int index = 1;
                foreach (var file in room.Files)
                {
                    //var fileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{Path.GetFileName(file.FileName)}";
                    var fileName = $"img{roomEntity.RoomId}{index++}.{file.FileName.Split('.')[1]}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    await _context.RoomImages.AddAsync(new RoomImage
                    {
                        RoomId = roomEntity.RoomId,
                        Image = $"/images/{fileName}"
                    });
                }

                await _context.SaveChangesAsync();
                _notyfService.Success("Tạo phòng thành công!");
                return RedirectToAction(nameof(Index), new { isActive = HttpContext.Session.GetInt32("RoomsParams") });
            }

            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "TypeName", room.RoomTypeId);
            return View(room);
        }

        // GET: Admin/Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
			var roomimage = await _context.RoomImages.ToListAsync();
            var viewModel = new RoomPostViewModel
            {
                RoomId = room.RoomId,
				RoomTypeId = room.RoomTypeId,
				Title = room.Title,
                Description = room.Description,
                Price   = room.Price,
                Area    = room.Area,
                NumRooms = room.NumRooms,
                NumBathrooms    = room.NumBathrooms,
                LastUpdate = DateTime.Now,
                IsActive = room.IsActive,
                StreetNumber= room.StreetNumber,
                Street  = room.Street,
                Ward = room.Ward,
                District= room.District,
                City= room.City,
				HasRefrigerator = room.HasRefrigerator,
				HasAirConditioner = room.HasAirConditioner,
				HasWasher = room.HasWasher,
				HasElevator = room.HasElevator,
				HasParking = room.HasParking,
				ViewCount = room.ViewCount,
			};
			ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "TypeName", room.RoomTypeId);
            return View(viewModel);
        }

        // POST: Admin/Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,RoomTypeId,Title,Description,Price,Area,NumRooms,NumBathrooms,CreateDate,LastUpdate,IsActive,StreetNumber,Street,Ward,District,City,HasRefrigerator,HasAirConditioner,HasWasher,HasElevator,HasParking,ViewCount,Files")] RoomPostViewModel room)
        {
            if (id != room.RoomId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    int result = await _context.Rooms.Where(x => x.RoomId == room.RoomId).ExecuteUpdateAsync(x =>
                    x.SetProperty(p => p.RoomTypeId, room.RoomTypeId)
                    .SetProperty(p => p.Title, room.Title)
                    .SetProperty(p => p.Description, room.Description)
                    .SetProperty(p => p.Price, room.Price)
                    .SetProperty(p => p.Area, room.Area)
                    .SetProperty(p => p.NumRooms, room.NumRooms)
                    .SetProperty(p => p.NumBathrooms, room.NumBathrooms)
                    .SetProperty(p => p.LastUpdate, DateTime.Now)
                    .SetProperty(p => p.IsActive, room.IsActive)
                    .SetProperty(p => p.StreetNumber, room.StreetNumber)
                    .SetProperty(p => p.Street, room.Street)
                    .SetProperty(p => p.Ward, room.Ward)
                    .SetProperty(p => p.District, room.District)
                    .SetProperty(p => p.City, room.City)
                    .SetProperty(p => p.HasRefrigerator, room.HasRefrigerator)
                    .SetProperty(p => p.HasAirConditioner, room.HasAirConditioner)
                    .SetProperty(p => p.HasWasher, room.HasWasher)
                    .SetProperty(p => p.HasElevator, room.HasElevator)
                    .SetProperty(p => p.HasParking, room.HasParking)
                    .SetProperty(p => p.ViewCount, room.ViewCount));

                    if (result > 0)
                    {
                        _notyfService.Success("Cập nhật phòng thành công!");
                    }
                    else
                    {
                        _notyfService.Success("Không tìm thấy room tương ứng!");
                    }

                    await _context.SaveChangesAsync();

                    int index = 1;

                    var images = await _context.RoomImages.Where(x => x.RoomId == room.RoomId).ToListAsync();
                    images.ForEach(item => _context.RoomImages.Remove(item));
                    await _context.SaveChangesAsync();

                    foreach (var file in room.Files)
                    {
                        var fileName = $"img{room.RoomId}-{index++}.{file.FileName.Split('.')[1]}";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        await _context.RoomImages.AddAsync(new RoomImage
                        {
                            RoomId = room.RoomId,
                            Image = $"/images/{fileName}"
                        });
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomId))
                    {
                        _notyfService.Success("Có lỗi xảy ra!");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { isActive = HttpContext.Session.GetInt32("RoomsParams") });
            }

            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "TypeName", room.RoomTypeId);
            return View(room);
        }

        // GET: Admin/Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.RoomsParams = (int)HttpContext.Session.GetInt32("RoomsParams");

            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }
            var roomImages = await _context.RoomImages
            .Where(ri => ri.RoomId == id)
            .ToListAsync();
            ViewBag.RoomImages = roomImages;

            return View(room);
        }
        // POST: Admin/Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rooms == null)
            {
                return Problem("Entity set 'DbmojoinContext.Rooms'  is null.");
            }
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa phòng thành công!");
            return RedirectToAction(nameof(Index), new { isActive = HttpContext.Session.GetInt32("RoomsParams") });
        }
        private bool RoomExists(int id)
        {
            return (_context.Rooms?.Any(e => e.RoomId == id)).GetValueOrDefault();
        }



        public IActionResult UpdateRoomActive(int roomId)
        {
            int isActive = (int)HttpContext.Session.GetInt32("RoomsParams");

            try
            {
                var findRoom = _context.Rooms.Where(x => x.RoomId == roomId).FirstOrDefault();
                if (findRoom != null)
                {
                    findRoom.IsActive = 1;
                    _context.SaveChanges();

                    _notyfService.Success("Cập nhật thành công!");
                }
                else
                {
                    _notyfService.Success("Cập nhật thất bại!");
                }

                var dbmojoinContext = GetListRoomByActive(isActive);

                return PartialView("ListRoom", dbmojoinContext);
            }
            catch
            {
                _notyfService.Success("Cập nhật thất bại!");
            }
            return RedirectToAction(nameof(Index), new { isActive });
        }


        public IActionResult DeleteRoom(int roomId)
        {
            int isActive = (int)HttpContext.Session.GetInt32("RoomsParams");

            try
            {
                var findRoom = _context.Rooms.Where(x => x.RoomId == roomId).FirstOrDefault();
                if (findRoom != null)
                {
                    findRoom.IsActive = 2;
                    _context.SaveChanges();

                    _notyfService.Success("Xóa thành công!");
                }
                else
                {
                    _notyfService.Success("Xóa thất bại!");
                }

                var dbmojoinContext = GetListRoomByActive(isActive);

                return PartialView("ListRoom", dbmojoinContext);
            }
            catch
            {
                _notyfService.Success("Xoa thất bại!");
            }
            return RedirectToAction(nameof(Index), new { isActive });
        }
        [HttpPost]
        public ActionResult UploadMultipleImage(List<IFormFile> files)
        {
            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);

                    //string path = Path.Combine(Server.MapPath("~/images/"), _FileName);
                    //if (System.IO.File.Exists(path))
                    //{
                    //    //nếu hình ảnh đã tồn tại, thì xóa ảnh cũ, cập nhật lại ảnh mới
                    //    System.IO.File.Delete(path);
                    //    file.SaveAs(path);
                    //}
                    //else
                    //{
                    //    file.SaveAs(path);
                    //}
                }
            }
            return RedirectToAction("Index");
        }
    }

}