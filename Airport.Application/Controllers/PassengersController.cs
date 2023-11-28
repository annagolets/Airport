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
    public class PassengersController : Controller
    {
        private readonly Context _context;

        public PassengersController(Context context)
        {
            _context = context;
        }

        // GET: Passengers
        public async Task<IActionResult> Index()
        {
              return _context.Passengers != null ? 
                          View(await _context.Passengers.ToListAsync()) :
                          Problem("Entity set 'Context.Passengers'  is null.");
        }

        // GET: Passengers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Passengers == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passengers.Include(x => x.Tickets)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (passenger == null)
            {
                return NotFound();
            }

            return View(passenger.Tickets);
        }

        // GET: Passengers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Passengers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fullname,PassportData")] Passenger passenger)
        {
            passenger.Id = Guid.NewGuid();
            _context.Add(passenger);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Passengers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Passengers == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passengers.FindAsync(id);
            if (passenger == null)
            {
                return NotFound();
            }
            return View(passenger);
        }

        // POST: Passengers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Fullname,PassportData")] Passenger passenger)
        {
            if (id != passenger.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(passenger);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PassengerExists(passenger.Id))
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

        // GET: Passengers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Passengers == null)
            {
                return NotFound();
            }

            var passenger = await _context.Passengers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (passenger == null)
            {
                return NotFound();
            }

            return View(passenger);
        }

        // POST: Passengers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Passengers == null)
            {
                return Problem("Entity set 'Context.Passengers'  is null.");
            }
            var passenger = await _context.Passengers.FindAsync(id);
            if (passenger != null)
            {
                _context.Passengers.Remove(passenger);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PassengerExists(Guid id)
        {
          return (_context.Passengers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
