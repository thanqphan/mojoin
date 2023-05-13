﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mojoin.Models;

namespace mojoin.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        public async Task<IActionResult> Index()
        {
            var dbmojoinContext = _context.Rooms.Include(r => r.RoomType);
            ViewData["QuyenAcc"] = new SelectList(_context.Roles, "RoleName", "RoleName");
            List<SelectListItem> lsTrangThai = new List<SelectListItem>();
            List<SelectListItem> lsLoaiPhong = new List<SelectListItem>();
            lsTrangThai.Add(new SelectListItem() { Text = "Đã duyệt", Value = "0" });
            lsTrangThai.Add(new SelectListItem() { Text = "Chưa duyệt", Value = "1" });
            lsTrangThai.Add(new SelectListItem() { Text = "Không được duyệt", Value = "2" });
            lsLoaiPhong.Add(new SelectListItem() { Text = "Nhà trọ", Value = "1" });
            lsLoaiPhong.Add(new SelectListItem() { Text = "Căn hộ", Value = "2" });
            lsLoaiPhong.Add(new SelectListItem() { Text = "Ở chung chủ", Value = "3" });
            lsLoaiPhong.Add(new SelectListItem() { Text = "Ở ghép", Value = "4" });
            ViewData["lsLoaiPhong"] = lsLoaiPhong;
            ViewData["lsTrangThai"] = lsTrangThai;
            //
            return View(await dbmojoinContext.ToListAsync());
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

            return View(room);
        }

        // GET: Admin/Rooms/Create
        public IActionResult Create()
        {
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeId");
            return View();
        }

        // POST: Admin/Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,RoomTypeId,Title,Description,Price,Area,NumRooms,NumBathrooms,CreateDate,LastUpdate,IsActive,StreetNumber,Street,Ward,District,City,HasRefrigerator,HasAirConditioner,HasWasher,HasElevator,HasParking,ViewCount")] RoomViewModel room)
        {
            if (ModelState.IsValid)
            {
                _context.Rooms.Add(new Room
                {
                    RoomTypeId = room.RoomTypeId,
                    Title = room.Title,
                    Description = room.Description,
                    Price = room.Price,
                    Area = room.Area,
                    NumRooms = room.NumRooms,
                    NumBathrooms = room.NumBathrooms,
                    IsActive = room.IsActive,
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
                    UserId = 5,
                    CreateDate = DateTime.Now
                });
                await _context.SaveChangesAsync();
                _notyfService.Success("Tạo phòng thành công!");
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeId", room.RoomTypeId);
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
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeId", room.RoomTypeId);
            return View(room);
        }

        // POST: Admin/Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,RoomTypeId,Title,Description,Price,Area,NumRooms,NumBathrooms,CreateDate,LastUpdate,IsActive,StreetNumber,Street,Ward,District,City,HasRefrigerator,HasAirConditioner,HasWasher,HasElevator,HasParking,ViewCount")] RoomViewModel room)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeId", room.RoomTypeId);
            return View(room);
        }

        // GET: Admin/Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
            return RedirectToAction(nameof(Index));
        }
        private bool RoomExists(int id)
        {
            return (_context.Rooms?.Any(e => e.RoomId == id)).GetValueOrDefault();
        }
    }
}
