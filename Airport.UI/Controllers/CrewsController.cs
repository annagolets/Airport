using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Airport.Shared.Models;
using Airport.Application.Data;

namespace Airport.Application.Controllers
{
    public class CrewsController : Controller
    {
        private readonly Context _context;

        public CrewsController(Context context)
        {
            _context = context;
        }

        // GET: Crews
        public async Task<IActionResult> Index()
        {
              return _context.Crews != null ? 
                          View(await _context.Crews.ToListAsync()) :
                          Problem("Entity set 'Context.Crews'  is null.");
        }

        // GET: Crews/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Crews == null)
            {
                return NotFound();
            }

            var crew = await _context.Crews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (crew == null)
            {
                return NotFound();
            }

            return View(crew);
        }

        // GET: Crews/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Crews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] Crew crew)
        {
            if (ModelState.IsValid)
            {
                crew.Id = Guid.NewGuid();
                _context.Add(crew);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(crew);
        }

        // GET: Crews/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Crews == null)
            {
                return NotFound();
            }

            var crew = await _context.Crews.FindAsync(id);
            if (crew == null)
            {
                return NotFound();
            }
            return View(crew);
        }

        // POST: Crews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id")] Crew crew)
        {
            if (id != crew.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(crew);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CrewExists(crew.Id))
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
            return View(crew);
        }

        // GET: Crews/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Crews == null)
            {
                return NotFound();
            }

            var crew = await _context.Crews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (crew == null)
            {
                return NotFound();
            }

            return View(crew);
        }

        // POST: Crews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Crews == null)
            {
                return Problem("Entity set 'Context.Crews'  is null.");
            }
            var crew = await _context.Crews.FindAsync(id);
            if (crew != null)
            {
                _context.Crews.Remove(crew);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CrewExists(Guid id)
        {
          return (_context.Crews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
