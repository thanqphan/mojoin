using System;
using System.Collections.Generic;
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
    [Authorize(Policy = "AdminOrStaff")]
    public class AdminController : Controller
    {
        private readonly DbmojoinContext _context;

        public AdminController(DbmojoinContext context)
        {
            _context = context;
        }

        // GET: Admin/TransactionHistory
        public IActionResult Index()
        {
            var history = _context.TransactionHistories.Include(th => th.User).ToList();
            List<SelectListItem> lstrangthai = new List<SelectListItem>();
            lstrangthai.Add(new SelectListItem() { Text = "  Thành công ", Value = "0" });
            lstrangthai.Add(new SelectListItem() { Text = "Thất bại", Value = "1" });
            lstrangthai.Add(new SelectListItem() { Text = "Đang xử lý", Value = "2" });


            ViewData["lstrangthaihoatdongg"] = lstrangthai;
            return View(history);
        }
    }
}


