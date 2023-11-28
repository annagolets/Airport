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
    public class RacesController : Controller
    {
        private readonly Context _context;

        public RacesController(Context context)
        {
            _context = context;
        }

        // GET: Races
        public async Task<IActionResult> Index()
        {
              return _context.Races != null ? 
                          View(await _context.Races.ToListAsync()) :
                          Problem("Entity set 'Context.Races'  is null.");
        }

        // GET: Races/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Races == null)
            {
                return NotFound();
            }

            var race = await _context.Races
                .FirstOrDefaultAsync(m => m.Id == id);
            if (race == null)
            {
                return NotFound();
            }

            return View(race);
        }

        // GET: Races/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Races/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateStart,DateEnd,StartPoint,EndPoint")] Race race)
        {
            if (ModelState.IsValid)
            {
                race.Id = Guid.NewGuid();
                _context.Add(race);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(race);
        }

        // GET: Races/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Races == null)
            {
                return NotFound();
            }

            var race = await _context.Races.FindAsync(id);
            if (race == null)
            {
                return NotFound();
            }
            return View(race);
        }

        // POST: Races/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,DateStart,DateEnd,StartPoint,EndPoint")] Race race)
        {
            if (id != race.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(race);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaceExists(race.Id))
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
            return View(race);
        }

        // GET: Races/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Races == null)
            {
                return NotFound();
            }

            var race = await _context.Races
                .FirstOrDefaultAsync(m => m.Id == id);
            if (race == null)
            {
                return NotFound();
            }

            return View(race);
        }

        // POST: Races/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Races == null)
            {
                return Problem("Entity set 'Context.Races'  is null.");
            }
            var race = await _context.Races.FindAsync(id);
            if (race != null)
            {
                _context.Races.Remove(race);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RaceExists(Guid id)
        {
          return (_context.Races?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
