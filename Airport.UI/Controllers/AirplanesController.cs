using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Airport.Shared.Models;
using Airport.Application.Data;
using System.Runtime.ConstrainedExecution;
using Airport.Application.Enums;
using X.PagedList;

namespace Airport.Application.Controllers
{
    public class AirplanesController : Controller
    {
        private readonly Context _context;

        private const int TOTAL_SET_COUNT = 15;

        public AirplanesController(Context context)
        {
            _context = context;
        }

        // GET: Airplanes
        public async Task<IActionResult> Index(AirplaneSort airplaneSort, string searchSpecifications, string currentFilter, int? page)
        {
            if (searchSpecifications != null)
            {
                page = 1;
            }
            else
            {
                searchSpecifications = currentFilter;
            }
            IQueryable<Airplane> context = _context.Airplanes.Include(a => a.TypeAirplane);
            context = Sort(context, airplaneSort);
            ViewBag.CurrentFilter = searchSpecifications;
            context = SearchInName(context, searchSpecifications);
            int pageNumber = page ?? 1;
            return View(context.ToPagedList(pageNumber, TOTAL_SET_COUNT));
        }

        // GET: Airplanes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Airplanes == null)
            {
                return NotFound();
            }

            var airplane = await _context.Airplanes
                .Include(a => a.TypeAirplane)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (airplane == null)
            {
                return NotFound();
            }

            return View(airplane);
        }

        // GET: Airplanes/Create
        public IActionResult Create()
        {
            ViewData["TypeAirplaneId"] = new SelectList(_context.TypeAirplanes, "Id", "Name");
            return View();
        }

        // POST: Airplanes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Mark,Capacity,TypeAirplaneId,Specifications,DateLastRepair")] Airplane airplane)
        {
            if (ModelState.IsValid)
            {
                airplane.Id = Guid.NewGuid();
                _context.Add(airplane);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeAirplaneId"] = new SelectList(_context.TypeAirplanes, "Id", "Name");
            return View(airplane);
        }

        // GET: Airplanes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Airplanes == null)
            {
                return NotFound();
            }

            var airplane = await _context.Airplanes.FindAsync(id);
            if (airplane == null)
            {
                return NotFound();
            }
            ViewData["TypeAirplaneId"] = new SelectList(_context.TypeAirplanes, "Id", "Name");
            return View(airplane);
        }

        // POST: Airplanes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Mark,Capacity,TypeAirplaneId,Specifications,DateLastRepair")] Airplane airplane)
        {
            if (id != airplane.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(airplane);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirplaneExists(airplane.Id))
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
            ViewData["TypeAirplaneId"] = new SelectList(_context.TypeAirplanes, "Id", "Name");
            return View(airplane);
        }

        // GET: Airplanes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Airplanes == null)
            {
                return NotFound();
            }

            var airplane = await _context.Airplanes
                .Include(a => a.TypeAirplane)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (airplane == null)
            {
                return NotFound();
            }

            return View(airplane);
        }

        // POST: Airplanes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Airplanes == null)
            {
                return Problem("Entity set 'Context.Airplanes'  is null.");
            }
            var airplane = await _context.Airplanes.FindAsync(id);
            if (airplane != null)
            {
                _context.Airplanes.Remove(airplane);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AirplaneExists(Guid id)
        {
          return (_context.Airplanes?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private IQueryable<Airplane> Sort(IQueryable<Airplane> airplanes, AirplaneSort airplaneSort)
        {
            ViewData["Capacity"] = airplaneSort == AirplaneSort.CapacityAsc ? AirplaneSort.CapacityDesc : AirplaneSort.CapacityAsc;
            ViewData["Mark"] = airplaneSort == AirplaneSort.MarkAsc ? AirplaneSort.MarkDesc : AirplaneSort.MarkAsc;
            ViewData["DateLastRepair"] = airplaneSort == AirplaneSort.DateLastRepairAsc ? AirplaneSort.DateLastRepairDesc : AirplaneSort.DateLastRepairAsc;
            ViewData["Specifications"] = airplaneSort == AirplaneSort.SpecificationsAsc ? AirplaneSort.SpecificationsDesc : AirplaneSort.SpecificationsAsc;
            airplanes = airplaneSort switch
            {
                AirplaneSort.CapacityAsc => airplanes.OrderBy(s => s.Capacity),
                AirplaneSort.CapacityDesc => airplanes.OrderByDescending(s => s.Capacity),
                AirplaneSort.MarkAsc => airplanes.OrderBy(s => s.Mark),
                AirplaneSort.MarkDesc => airplanes.OrderByDescending(s => s.Mark),
                AirplaneSort.DateLastRepairAsc => airplanes.OrderBy(s => s.DateLastRepair),
                AirplaneSort.DateLastRepairDesc => airplanes.OrderByDescending(s => s.DateLastRepair),
                AirplaneSort.SpecificationsAsc => airplanes.OrderBy(s => s.Specifications),
                AirplaneSort.SpecificationsDesc => airplanes.OrderByDescending(s => s.Specifications),
                _ => airplanes.OrderBy(s => s.DateLastRepair),
            };
            return airplanes;
        }

        private IQueryable<Airplane> SearchInName(IQueryable<Airplane> airplanes, string searchSpecifications)
        {
            if (!String.IsNullOrEmpty(searchSpecifications))
            {
                airplanes = airplanes.Where(s => s.Specifications.Contains(searchSpecifications));
            }
            return airplanes;
        }
    }
}
