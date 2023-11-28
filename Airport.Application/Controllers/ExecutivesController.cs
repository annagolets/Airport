using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Airport.Application.Data;
using Airport.Shared.Models;
using X.PagedList;

namespace Airport.Application.Controllers
{
    public class ExecutivesController : Controller
    {
        private readonly Context _context;

        private const int TOTAL_SET_COUNT = 15;

        public ExecutivesController(Context context)
        {
            _context = context;
        }

        // GET: Executives
        public async Task<IActionResult> Index(JobTitle jobTitle, JobTitle currentFilter, int? page)
        {
            if (jobTitle != JobTitle.Default)
            {
                page = 1;
            }
            else
            {
                jobTitle = currentFilter;
            }
            IQueryable<Executive> context = _context.Executives.Include(e => e.Crew);
            ViewBag.CurrentFilter = jobTitle;
            context = SearchInJobTitle(context, jobTitle);
            int pageNumber = page ?? 1;
            return View(context.ToPagedList(pageNumber, TOTAL_SET_COUNT));
        }

        // GET: Executives/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Executives == null)
            {
                return NotFound();
            }

            var executive = await _context.Executives
                .Include(e => e.Crew)
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
            ViewData["CrewId"] = new SelectList(_context.Crews, "Id", "TeamName");
            return View();
        }

        // POST: Executives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fullname,JobTitle,CrewId")] Executive executive)
        {
            executive.Id = Guid.NewGuid();
            _context.Add(executive);
            await _context.SaveChangesAsync();
            ViewData["CrewId"] = new SelectList(_context.Crews, "Id", "TeamName", executive.CrewId);
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
            ViewData["CrewId"] = new SelectList(_context.Crews, "Id", "TeamName", executive.CrewId);
            return View(executive);
        }

        // POST: Executives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Fullname,JobTitle,CrewId")] Executive executive)
        {
            if (id != executive.Id)
            {
                return NotFound();
            }

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
            ViewData["CrewId"] = new SelectList(_context.Crews, "Id", "TeamName", executive.CrewId);
            return RedirectToAction(nameof(Index));
        }

        // GET: Executives/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Executives == null)
            {
                return NotFound();
            }

            var executive = await _context.Executives
                .Include(e => e.Crew)
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
        private IQueryable<Executive> SearchInJobTitle(IQueryable<Executive> executives, JobTitle jobTitle)
        {
            if (jobTitle != JobTitle.Default)
            {
                executives = executives.Where(s => s.JobTitle == jobTitle);
            }
            return executives;
        }
    }
}
