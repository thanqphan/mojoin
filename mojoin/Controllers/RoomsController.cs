using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mojoin.Models;

namespace mojoin.Controllers
{
    public class RoomsController : Controller
    {
        private readonly DbmojoinContext _context;

        public RoomsController(DbmojoinContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var dbmojoinContext = _context.Rooms.Include(r => r.Address).Include(r => r.RoomType).Include(r => r.Status).Include(r => r.User);
            return View(await dbmojoinContext.ToListAsync());
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Address)
                .Include(r => r.RoomType)
                .Include(r => r.Status)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            ViewData["AddressId"] = new SelectList(_context.RoomAddresses, "AddressId", "AddressId");
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeId");
            ViewData["StatusId"] = new SelectList(_context.RoomStatuses, "StatusId", "StatusId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,Title,Description,Price,Area,NumRooms,CreateDate,LastUpdate,Image1,Image2,Image3,Image4,RoomTypeId,UserId,AddressId,StatusId")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.RoomAddresses, "AddressId", "AddressId", room.AddressId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeId", room.RoomTypeId);
            ViewData["StatusId"] = new SelectList(_context.RoomStatuses, "StatusId", "StatusId", room.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", room.UserId);
            return View(room);
        }

        // GET: Rooms/Edit/5
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
            ViewData["AddressId"] = new SelectList(_context.RoomAddresses, "AddressId", "AddressId", room.AddressId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeId", room.RoomTypeId);
            ViewData["StatusId"] = new SelectList(_context.RoomStatuses, "StatusId", "StatusId", room.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", room.UserId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,Title,Description,Price,Area,NumRooms,CreateDate,LastUpdate,Image1,Image2,Image3,Image4,RoomTypeId,UserId,AddressId,StatusId")] Room room)
        {
            if (id != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomId))
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
            ViewData["AddressId"] = new SelectList(_context.RoomAddresses, "AddressId", "AddressId", room.AddressId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeId", room.RoomTypeId);
            ViewData["StatusId"] = new SelectList(_context.RoomStatuses, "StatusId", "StatusId", room.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", room.UserId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.Address)
                .Include(r => r.RoomType)
                .Include(r => r.Status)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
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
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
          return (_context.Rooms?.Any(e => e.RoomId == id)).GetValueOrDefault();
        }
    }
}
