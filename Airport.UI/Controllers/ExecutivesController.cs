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
    public class ExecutivesController : Controller
    {
        private readonly Context _context;

        public ExecutivesController(Context context)
        {
            _context = context;
        }

        // GET: Executives
        public async Task<IActionResult> Index()
        {
              return _context.Executives != null ? 
                          View(await _context.Executives.ToListAsync()) :
                          Problem("Entity set 'Context.Executives'  is null.");
        }

        // GET: Executives/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Executives == null)
            {
                return NotFound();
            }

            var executive = await _context.Executives
                .FirstOrDefaultAsync(m => m.Id == id);
            if (executive == null)
            {
                return NotFound();
            }

            return View(executive);
        }

        // GET: Executives/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Executives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fullname,JobTitle")] Executive executive)
        {
            if (ModelState.IsValid)
            {
                executive.Id = Guid.NewGuid();
                _context.Add(executive);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(executive);
        }

        // GET: Executives/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Executives == null)
            {
                return NotFound();
            }

            var executive = await _context.Executives.FindAsync(id);
            if (executive == null)
            {
                return NotFound();
            }
            return View(executive);
        }

        // POST: Executives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Fullname,JobTitle")] Executive executive)
        {
            if (id != executive.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(executive);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExecutiveExists(executive.Id))
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
            return View(executive);
        }

        // GET: Executives/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Executives == null)
            {
                return NotFound();
            }

            var executive = await _context.Executives
                .FirstOrDefaultAsync(m => m.Id == id);
            if (executive == null)
            {
                return NotFound();
            }

            return View(executive);
        }

        // POST: Executives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Executives == null)
            {
                return Problem("Entity set 'Context.Executives'  is null.");
            }
            var executive = await _context.Executives.FindAsync(id);
            if (executive != null)
            {
                _context.Executives.Remove(executive);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExecutiveExists(Guid id)
        {
          return (_context.Executives?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
