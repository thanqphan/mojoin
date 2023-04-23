using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mojoin.Models;

namespace mojoin.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserAccountsController : Controller
    {
        private readonly DbmojoinContext _context;
        public INotyfService _notyfService { get; }

        public UserAccountsController(DbmojoinContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/UserAccounts
        public async Task<IActionResult> Index()
        {
            ViewData["QuyenAcc"] = new SelectList(_context.Roles, "RoleName", "RoleName");
            List<SelectListItem> lsTrangThai = new List<SelectListItem>();
            lsTrangThai.Add(new SelectListItem() { Text = "Hoạt động", Value = "1" });
            lsTrangThai.Add(new SelectListItem() { Text = "Tạm ngưng", Value = "0" });
            ViewData["lsTrangThai"] = lsTrangThai;
            //
            var dbmojoinContext = _context.UserAccounts.Include(u => u.Role);
            return View(await dbmojoinContext.ToListAsync());
        }

        // GET: Admin/UserAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserAccounts == null)
            {
                return NotFound();
            }

            var userAccount = await _context.UserAccounts
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (userAccount == null)
            {
                return NotFound();
            }

            return View(userAccount);
        }

        // GET: Admin/UserAccounts/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            List<SelectListItem> lstActive = new List<SelectListItem>();
            lstActive.Add(new SelectListItem() { Text = "Hoạt động", Value = "1" });
            lstActive.Add(new SelectListItem() { Text = "Tạm ngưng", Value = "0" });
            ViewData["lstActive"] = lstActive;
            return View();
        }

        // POST: Admin/UserAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Username,Password,Phone,Avatar,IsActive,RoleId")] UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userAccount);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm mới thành công!");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Warning("Thêm mới không thành công!");
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            List<SelectListItem> lstActive = new List<SelectListItem>();
            lstActive.Add(new SelectListItem() { Text = "Hoạt động", Value = "1" });
            lstActive.Add(new SelectListItem() { Text = "Tạm ngưng", Value = "0" });
            ViewData["lstActive"] = lstActive;
            return View(userAccount);
        }

        // GET: Admin/UserAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserAccounts == null)
            {
                return NotFound();
            }

            var userAccount = await _context.UserAccounts.FindAsync(id);
            if (userAccount == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", userAccount.RoleId);
            return View(userAccount);
        }

        // POST: Admin/UserAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,Username,Password,Phone,Avatar,IsActive,RoleId")] UserAccount userAccount)
        {
            if (id != userAccount.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userAccount);
                    await _context.SaveChangesAsync(); 
                    _notyfService.Success("Chỉnh sửa thành công!");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAccountExists(userAccount.AccountId))
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
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", userAccount.RoleId);
            return View(userAccount);
        }

        // GET: Admin/UserAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserAccounts == null)
            {
                return NotFound();
            }

            var userAccount = await _context.UserAccounts
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (userAccount == null)
            {
                return NotFound();
            }

            return View(userAccount);
        }

        // POST: Admin/UserAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserAccounts == null)
            {
                return Problem("Entity set 'DbmojoinContext.UserAccounts'  is null.");
            }
            var userAccount = await _context.UserAccounts.FindAsync(id);
            if (userAccount != null)
            {
                _context.UserAccounts.Remove(userAccount);
            }
            _notyfService.Success("Xóa thành công!");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserAccountExists(int id)
        {
          return (_context.UserAccounts?.Any(e => e.AccountId == id)).GetValueOrDefault();
        }
    }
}
