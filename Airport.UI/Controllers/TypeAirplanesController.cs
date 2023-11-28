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
    public class TypeAirplanesController : Controller
    {
        private readonly Context _context;

        public TypeAirplanesController(Context context)
        {
            _context = context;
        }

        // GET: TypeAirplanes
        public async Task<IActionResult> Index()
        {
              return _context.TypeAirplanes != null ? 
                          View(await _context.TypeAirplanes.ToListAsync()) :
                          Problem("Entity set 'Context.TypeAirplanes'  is null.");
        }

        // GET: TypeAirplanes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TypeAirplanes == null)
            {
                return NotFound();
            }

            var typeAirplane = await _context.TypeAirplanes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeAirplane == null)
            {
                return NotFound();
            }

            return View(typeAirplane);
        }

        // GET: TypeAirplanes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeAirplanes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Appointment,Limit")] TypeAirplane typeAirplane)
        {
            if (ModelState.IsValid)
            {
                typeAirplane.Id = Guid.NewGuid();
                _context.Add(typeAirplane);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeAirplane);
        }

        // GET: TypeAirplanes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TypeAirplanes == null)
            {
                return NotFound();
            }

            var typeAirplane = await _context.TypeAirplanes.FindAsync(id);
            if (typeAirplane == null)
            {
                return NotFound();
            }
            return View(typeAirplane);
        }

        // POST: TypeAirplanes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Appointment,Limit")] TypeAirplane typeAirplane)
        {
            if (id != typeAirplane.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeAirplane);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeAirplaneExists(typeAirplane.Id))
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
            return View(typeAirplane);
        }

        // GET: TypeAirplanes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TypeAirplanes == null)
            {
                return NotFound();
            }

            var typeAirplane = await _context.TypeAirplanes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeAirplane == null)
            {
                return NotFound();
            }

            return View(typeAirplane);
        }

        // POST: TypeAirplanes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TypeAirplanes == null)
            {
                return Problem("Entity set 'Context.TypeAirplanes'  is null.");
            }
            var typeAirplane = await _context.TypeAirplanes.FindAsync(id);
            if (typeAirplane != null)
            {
                _context.TypeAirplanes.Remove(typeAirplane);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeAirplaneExists(Guid id)
        {
          return (_context.TypeAirplanes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
