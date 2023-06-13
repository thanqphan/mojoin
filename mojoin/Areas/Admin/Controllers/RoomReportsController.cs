using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mojoin.Models;

namespace mojoin.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomReportsController : Controller
    {
        private readonly DbmojoinContext _context;

        public RoomReportsController(DbmojoinContext context)
        {
            _context = context;
        }

        // GET: Admin/RoomReports
        public async Task<IActionResult> Index()
        {
            var dbmojoinContext = _context.RoomReports.Include(r => r.Room).Include(r => r.User);
            return View(await dbmojoinContext.ToListAsync());
        }

        // GET: Admin/RoomReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RoomReports == null)
            {
                return NotFound();
            }

            var roomReport = await _context.RoomReports
                .Include(r => r.Room)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (roomReport == null)
            {
                return NotFound();
            }

            return View(roomReport);
        }

        // GET: Admin/RoomReports/Create
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Admin/RoomReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportId,RoomId,UserId,ReportContent,IsResolved,CreateDate")] RoomReport roomReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roomReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", roomReport.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", roomReport.UserId);
            return View(roomReport);
        }

        // GET: Admin/RoomReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RoomReports == null)
            {
                return NotFound();
            }

            var roomReport = await _context.RoomReports.FindAsync(id);
            if (roomReport == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", roomReport.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", roomReport.UserId);
            return View(roomReport);
        }

        // POST: Admin/RoomReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportId,RoomId,UserId,ReportContent,IsResolved,CreateDate")] RoomReport roomReport)
        {
            if (id != roomReport.ReportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomReportExists(roomReport.ReportId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", roomReport.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", roomReport.UserId);
            return View(roomReport);
        }

        // GET: Admin/RoomReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RoomReports == null)
            {
                return NotFound();
            }

            var roomReport = await _context.RoomReports
                .Include(r => r.Room)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (roomReport == null)
            {
                return NotFound();
            }

            return View(roomReport);
        }

        // POST: Admin/RoomReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RoomReports == null)
            {
                return Problem("Entity set 'DbmojoinContext.RoomReports'  is null.");
            }
            var roomReport = await _context.RoomReports.FindAsync(id);
            if (roomReport != null)
            {
                _context.RoomReports.Remove(roomReport);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomReportExists(int id)
        {
          return (_context.RoomReports?.Any(e => e.ReportId == id)).GetValueOrDefault();
        }
    }
}
