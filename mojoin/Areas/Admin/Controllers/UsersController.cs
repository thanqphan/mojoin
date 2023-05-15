using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mojoin.Models;

namespace mojoin.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Staff,Admin", Policy = "StaffOnly")]
    public class UsersController : Controller
    {
        private readonly DbmojoinContext _context;
        public INotyfService _notyfService { get; }

        public UsersController(DbmojoinContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index(int isActive = -1)
        {
            HttpContext.Session.SetInt32("Users1", isActive);
            ViewBag.Users1 = isActive;
            var dbmojoinContext = isActive == -1 ?
                _context.Users.Include(r => r.RoomType) :
                _context.Users.Include(u => u.RoomType).Where(x => x.IsActive == isActive);

            ViewData["QuyenAcc"] = new SelectList(_context.Roles, "RoleName", "RoleName");
            List<SelectListItem> lsTrangThaiHoatDong = new List<SelectListItem>();

            lsTrangThaiHoatDong.Add(new SelectListItem() { Text = "Đang hoạt động", Value = "0" });
            lsTrangThaiHoatDong.Add(new SelectListItem() { Text = "Tạm ngưng", Value = "1" });
            
            ViewData["lsTrangThaiHoatDong"] = lsTrangThaiHoatDong;
            //
            return View(await dbmojoinContext.ToListAsync());
        }

        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Admin/Users/Create
        public IActionResult Create()
        {
            ViewData["RolesId"] = new SelectList(_context.Roles, "RolelD", "RolelD");
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RolesId,Fullname,Phone,Email,Address,Sex,Dateofbirth,Password,Avatar,IsActive,InfoZalo,InfoFacebook,SupportUserId,CreateDate")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công!");
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,RolesId,Fullname,Phone,Email,Address,Sex,Dateofbirth,Password,Avatar,IsActive,InfoZalo,InfoFacebook,SupportUserId,CreateDate")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật thành công!");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        _notyfService.Error("Có lỗi xảy ra!");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'DbmojoinContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công!");
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
