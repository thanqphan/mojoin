using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mojoin.Models;
using mojoin.ViewModel;

namespace mojoin.Controllers
{
    public class ProfileUserViewModelsController : Controller
    {
        private readonly DbmojoinContext _context;

        public ProfileUserViewModelsController(DbmojoinContext context)
        {
            _context = context;
        }

        // GET: ProfileUserViewModels
        public async Task<IActionResult> Index()
        {
              return _context.ProfileUserViewModel != null ? 
                          View(await _context.ProfileUserViewModel.ToListAsync()) :
                          Problem("Entity set 'DbmojoinContext.ProfileUserViewModel'  is null.");
        }

        // GET: ProfileUserViewModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProfileUserViewModel == null)
            {
                return NotFound();
            }

            var profileUserViewModel = await _context.ProfileUserViewModel
                .FirstOrDefaultAsync(m => m.UsserId == id);
            if (profileUserViewModel == null)
            {
                return NotFound();
            }

            return View(profileUserViewModel);
        }

        // GET: ProfileUserViewModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProfileUserViewModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsserId,FirstName,LastName,Phone,Email,Address,Sex,Dateofbirth,Avatar,SupportUserId")] ProfileUserViewModel profileUserViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(profileUserViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(profileUserViewModel);
        }

        // GET: ProfileUserViewModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProfileUserViewModel == null)
            {
                return NotFound();
            }

            var profileUserViewModel = await _context.ProfileUserViewModel.FindAsync(id);
            if (profileUserViewModel == null)
            {
                return NotFound();
            }
            return View(profileUserViewModel);
        }

        // POST: ProfileUserViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsserId,FirstName,LastName,Phone,Email,Address,Sex,Dateofbirth,Avatar,SupportUserId")] ProfileUserViewModel profileUserViewModel)
        {
            if (id != profileUserViewModel.UsserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profileUserViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileUserViewModelExists(profileUserViewModel.UsserId))
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
            return View(profileUserViewModel);
        }

        // GET: ProfileUserViewModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProfileUserViewModel == null)
            {
                return NotFound();
            }

            var profileUserViewModel = await _context.ProfileUserViewModel
                .FirstOrDefaultAsync(m => m.UsserId == id);
            if (profileUserViewModel == null)
            {
                return NotFound();
            }

            return View(profileUserViewModel);
        }

        // POST: ProfileUserViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProfileUserViewModel == null)
            {
                return Problem("Entity set 'DbmojoinContext.ProfileUserViewModel'  is null.");
            }
            var profileUserViewModel = await _context.ProfileUserViewModel.FindAsync(id);
            if (profileUserViewModel != null)
            {
                _context.ProfileUserViewModel.Remove(profileUserViewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileUserViewModelExists(int id)
        {
          return (_context.ProfileUserViewModel?.Any(e => e.UsserId == id)).GetValueOrDefault();
        }
    }
}
