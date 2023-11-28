using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Airport.Application.Data;
using Airport.Shared.Models;
using Airport.Application.Enums;
using X.PagedList;

namespace Airport.Application.Controllers
{
    public class RacesController : Controller
    {
        private readonly Context _context;

        public RacesController(Context context)
        {
            _context = context;
        }

        private const int TOTAL_SET_COUNT = 15;

        // GET: Races
        public async Task<IActionResult> Index(
            RaceSort raceSort,
            DateTime dateStart,
            DateTime dateEnd,
            DateTime currentDateStart,
            DateTime currentDateEnd,
            int? page)
        {
            if (dateStart != default || dateEnd != default)
            {
                page = 1;
            }
            else
            {
                dateStart = currentDateStart;
                dateEnd = currentDateEnd;
            }
            ViewBag.CurrentDateStart = dateStart;
            ViewBag.CurrentDateEnd = dateEnd;
            IQueryable<Race> context = _context.Races.Include(r => r.Airplane).Include(r => r.Crew);
            context = Sort(context, raceSort);
            ViewBag.CurrentSort = raceSort;

            context = DateFrom(context, dateStart);
            context = DateBefore(context, dateEnd);

            int pageNumber = page ?? 1;

            return View(await context.ToPagedListAsync(pageNumber, TOTAL_SET_COUNT));
        }

        // GET: Races/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Races == null)
            {
                return NotFound();
            }

            var race = await _context.Races
                .Include(r => r.Airplane)
                .Include(r => r.Crew)
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
            ViewData["AirplaneId"] = new SelectList(_context.Airplanes, "Id", "Mark");
            ViewData["CrewId"] = new SelectList(_context.Crews, "Id", "TeamName");
            return View();
        }

        // POST: Races/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumberRace,DateStart,DateEnd,StartPoint,EndPoint,AirplaneId,CrewId")] Race race)
        {
            race.Id = Guid.NewGuid();
            _context.Add(race);
            await _context.SaveChangesAsync();
            ViewData["AirplaneId"] = new SelectList(_context.Airplanes, "Id", "Mark", race.AirplaneId);
            ViewData["CrewId"] = new SelectList(_context.Crews, "Id", "TeamName", race.CrewId);
            return RedirectToAction(nameof(Index));
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
            ViewData["AirplaneId"] = new SelectList(_context.Airplanes, "Id", "Mark", race.AirplaneId);
            ViewData["CrewId"] = new SelectList(_context.Crews, "Id", "TeamName", race.CrewId);
            return View(race);
        }

        // POST: Races/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,NumberRace,DateStart,DateEnd,StartPoint,EndPoint,AirplaneId,CrewId")] Race race)
        {
            if (id != race.Id)
            {
                return NotFound();
            }

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
            ViewData["AirplaneId"] = new SelectList(_context.Airplanes, "Id", "Mark", race.AirplaneId);
            ViewData["CrewId"] = new SelectList(_context.Crews, "Id", "TeamName", race.CrewId);
            return RedirectToAction(nameof(Index));
        }

        // GET: Races/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Races == null)
            {
                return NotFound();
            }

            var race = await _context.Races
                .Include(r => r.Airplane)
                .Include(r => r.Crew)
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

        private IQueryable<Race> Sort(IQueryable<Race> races, RaceSort raceSort)
        {
            ViewData["StartPoint"] = raceSort == RaceSort.StartPointAsc ? RaceSort.StartPointDesc : RaceSort.StartPointAsc;
            ViewData["EndPoint"] = raceSort == RaceSort.EndPointAsc ? RaceSort.EndPointDesc : RaceSort.EndPointAsc;
            ViewData["DateEnd"] = raceSort == RaceSort.DateEndAsc ? RaceSort.DateEndDesc : RaceSort.DateEndAsc;
            ViewData["DateStart"] = raceSort == RaceSort.DateStartAsc ? RaceSort.DateStartDesc : RaceSort.DateStartAsc;
            races = raceSort switch
            {
                RaceSort.StartPointAsc => races.OrderBy(s => s.StartPoint),
                RaceSort.StartPointDesc => races.OrderByDescending(s => s.StartPoint),
                RaceSort.EndPointAsc => races.OrderBy(s => s.EndPoint),
                RaceSort.EndPointDesc => races.OrderByDescending(s => s.EndPoint),
                RaceSort.DateEndAsc => races.OrderBy(s => s.DateEnd),
                RaceSort.DateEndDesc => races.OrderByDescending(s => s.DateEnd),
                RaceSort.DateStartAsc => races.OrderBy(s => s.DateStart),
                RaceSort.DateStartDesc => races.OrderByDescending(s => s.DateStart),
                _ => races.OrderBy(s => s.DateStart),
            };
            return races;
        }

        private IQueryable<Race> DateFrom(IQueryable<Race> races, DateTime dateStart)
        {
            if (dateStart != default)
            {
                races = races.Where(s => s.DateStart > dateStart);
            }
            return races;
        }
        private IQueryable<Race> DateBefore(IQueryable<Race> races, DateTime dateEnd)
        {
            if (dateEnd != default)
            {
                races = races.Where(s => s.DateEnd < dateEnd);
            }
            return races;
        }
    }
}
