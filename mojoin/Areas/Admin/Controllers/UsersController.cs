using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mojoin.Helper;
using mojoin.Models;

namespace mojoin.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminOrStaff")]
    public class UsersController : Controller
    {
        private readonly DbmojoinContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public INotyfService _notyfService { get; }

        public UsersController(DbmojoinContext context, INotyfService notyfService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _notyfService = notyfService;
            _webHostEnvironment = webHostEnvironment;
        }

        //GET: Admin/Users
        public async Task<IActionResult> Index(int? roleFilter)
        {
            IQueryable<User> users = _context.Users;

            if (roleFilter.HasValue)
            {
                users = users.Where(u => u.RolesId == roleFilter);
            }

            ViewData["lstRole"] = GetRoleAcc();
            ViewData["lstrangthaihoatdong"] = GetTrangThaiHoatDong();

            return View(await users.ToListAsync());
        }

        private List<SelectListItem> GetRoleAcc()
        {
            List<SelectListItem> lstRole = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Admin", Value = "1" },
                new SelectListItem() { Text = "Staff", Value = "2" },
                new SelectListItem() { Text = "User", Value = "3" }
            };

            return lstRole;
        }

        private List<SelectListItem> GetTrangThaiHoatDong()
        {
            List<SelectListItem> lstrangthaihoatdong = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Đang hoạt động", Value = "0" },
                new SelectListItem() { Text = "Tạm ngưng", Value = "1" }
            };

            return lstrangthaihoatdong;
        }

        //GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            var userImages = await _context.Users
            .Where(ri => ri.UserId == id)
            .ToListAsync();
            ViewBag.Users = userImages;
            return View(user);
        }

        // GET: Admin/Users/Create
        public IActionResult Create()
        {
            ViewData["RolesId"] = new SelectList(_context.Roles, "RolelD", "RoleName");
            return View();
        }

        // POST: Admin/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User tk)
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
                return View(tk);
            }
            if (ModelState.IsValid)
            {
                User user = new User

                {
                    RolesId = tk.RolesId,
                    FirstName = tk.FirstName,
                    LastName = tk.LastName,
                    Phone = tk.Phone,
                    Email = tk.Email,
                    Address = tk.Address,
                    Sex = tk.Sex,
                    Dateofbirth = tk.Dateofbirth,
                    Password = tk.Password,
                    IsActive = true,
                    InfoFacebook = tk.InfoFacebook,
                    InfoZalo = tk.InfoZalo,
                    CreateDate = DateTime.Now,
                    ResetPasswordToken = null,
                };
                _context.Add(user);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công!");
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuyenTruyCap"] = new SelectList(_context.Roles, "RolesId", "RoleName", tk.RolesId);
            return RedirectToAction("Index");
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
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
        // POST: Admin/Staff/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,RolesId,FirstName,LastName,Phone,Email,Address,Sex,Dateofbirth,Password,ResetPasswordToken,Salt,Avatar,IsActive,InfoZalo,InfoFacebook,GoogleId,SupportUserId,CreateDate")] User user)
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
                    .SetProperty(p=>p.ResetPasswordToken,user.ResetPasswordToken)
                    .SetProperty(p => p.Avatar, user.Avatar)
                    .SetProperty(p => p.IsActive, user.IsActive)
                    .SetProperty(p => p.InfoZalo, user.InfoZalo)
                    .SetProperty(p => p.InfoFacebook, user.InfoFacebook)
                    .SetProperty(p => p.SupportUserId, user.SupportUserId)
                    .SetProperty(p => p.CreateDate, DateTime.Now));

                    if (result > 0)
                    {
                        _notyfService.Success("Cập nhật người dùng thành công!");
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
            ViewData["RolesId"] = new SelectList(_context.Roles, "RolelD", "RolelD", user.RolesId); return View(user);
        }

        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            var userImages = await _context.Users
            .Where(ri => ri.UserId == id)
            .ToListAsync();
            ViewBag.Users = userImages;
            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công!");
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                       + "_"
                       + Guid.NewGuid().ToString().Substring(0, 4)
                       + Path.GetExtension(fileName);
        }
    }
}
