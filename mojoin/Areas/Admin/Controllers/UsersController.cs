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
  
    /*    [Authorize(Roles = "Staff,Admin", Policy = "StaffOnly")]
    */
    public class UsersController : Controller
    {
        private readonly DbmojoinContext _context;
        public INotyfService _notyfService { get; }

        public UsersController(DbmojoinContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        //GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            

            ViewData["QuyenAcc"] = new SelectList(_context.Users, "RoleName", "RoleName");
            List<SelectListItem> lstrangthaihoatdong = new List<SelectListItem>();
            lstrangthaihoatdong.Add(new SelectListItem() { Text = "Đang hoạt động", Value = "0" });
            lstrangthaihoatdong.Add(new SelectListItem() { Text = "Tạm ngưng", Value = "1" });
           
            ViewData["lstrangthaihoatdong"] = lstrangthaihoatdong;

           return _context.Users != null ?
                        View(await _context.Users.ToListAsync()) :
                        Problem("Entity set 'DbmojoinContext.Roles'  is null.");
            
        }
        //GET: Admin/Users
        

        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
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
            ViewData["RolesId"] = new SelectList(_context.Roles, "RolelD", "RoleName");
            return View();
        }
        
        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( User taiKhoan)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    RolesId=taiKhoan.RolesId,
                    FirstName=taiKhoan.FirstName,   
                    LastName=taiKhoan.LastName,
                    Phone=taiKhoan.Phone,
                    Email=taiKhoan.Email,
                    Address = taiKhoan.Address,
                    Sex = taiKhoan.Sex,
                    Dateofbirth = taiKhoan.Dateofbirth,
                    Password = taiKhoan.Password,
                    IsActive = true,
                    InfoFacebook = taiKhoan.InfoFacebook,
                    InfoZalo = taiKhoan.InfoZalo,
                    CreateDate =DateTime.Now,
                };
                _context.Add(user);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công!");
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RolesId", "RoleName", taiKhoan.RolesId);
            return View(taiKhoan);
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
         
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RoleId", "RoleName", user.RolesId);
            return View(user);
        }
        

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,RolesId,FirstName,LastName,Phone,Email,Address,Sex,Dateofbirth,Password,Salt,Avatar,IsActive,InfoZalo,InfoFacebook,GoogleId,SupportUserId,CreateDate")] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int result = await _context.Users.Where(x => x.UserId == user.UserId).ExecuteUpdateAsync(x =>
                    x.SetProperty(p => p.RolesId, user.RolesId)
                    .SetProperty(p => p.FirstName, user.FirstName)
                    .SetProperty(p => p.LastName, user.LastName)
                    .SetProperty(p => p.Phone, user.Phone)
                    .SetProperty(p => p.Email, user.Email)
                    .SetProperty(p => p.Address, user.Address)
                    .SetProperty(p => p.Sex, user.Sex)
                    .SetProperty(p => p.Dateofbirth, user.Dateofbirth)
                    .SetProperty(p => p.Password, user.Password)
                    .SetProperty(p => p.Avatar, user.Avatar)
                    .SetProperty(p => p.IsActive, user.IsActive)
                    .SetProperty(p => p.InfoZalo, user.InfoZalo)
                    .SetProperty(p => p.InfoFacebook, user.InfoFacebook)
                    .SetProperty(p => p.SupportUserId, user.SupportUserId)
                    .SetProperty(p => p.CreateDate, DateTime.Now));

                    if (result > 0)
                    {
                        _notyfService.Success("Cập nhật người dùng thành công!");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notyfService.Success("Không tìm thấy người dùng tương ứng!");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            ViewData["RolesId"] = new SelectList(_context.Roles, "RolelD", "RolelD", user.RolesId);
            _notyfService.Error("Có lỗi xảy ra.");
            return View(user);

        }
        
        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
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
                return Problem("Entity set 'DbmojoinContext.Roles'  is null.");
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
          return _context.Users.Any(e => e.UserId == id);
        }
    }
}
