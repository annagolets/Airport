﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Airport.Application.Data;
using Airport.Shared.Models;
using X.PagedList;
using System.Net.Sockets;

namespace Airport.Application.Controllers
{
    public class TicketsController : Controller
    {
        private readonly Context _context;

        private const int TOTAL_SET_COUNT = 15;

        public TicketsController(Context context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(string searchName, string currentFilter, int? page)
        {
            if (searchName != null)
            {
                page = 1;
            }
            else
            {
                searchName = currentFilter;
            }

            IQueryable<Ticket> context = _context.Tickets.Include(t => t.Passenger).Include(t => t.Race);

            ViewBag.CurrentFilter = searchName;
            context = SearchInName(context, searchName);
            int pageNumber = page ?? 1;

            return View(context.ToPagedList(pageNumber, TOTAL_SET_COUNT));
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Passenger)
                .Include(t => t.Race)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["PassengerId"] = new SelectList(_context.Passengers, "Id", "Fullname");
            ViewData["RaceId"] = new SelectList(_context.Races, "Id", "NumberRace");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumberSeat,Cost,PassengerId, RaceId")] Ticket ticket)
        {
            ticket.Id = Guid.NewGuid();
            _context.Add(ticket);
            await _context.SaveChangesAsync();
            ViewData["PassengerId"] = new SelectList(_context.Passengers, "Id", "Fullname", ticket.PassengerId);
            ViewData["RaceId"] = new SelectList(_context.Races, "Id", "NumberRace", ticket.RaceId);
            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["PassengerId"] = new SelectList(_context.Passengers, "Id", "Fullname", ticket.PassengerId);
            ViewData["RaceId"] = new SelectList(_context.Races, "Id", "NumberRace", ticket.RaceId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,NumberSeat,Cost,PassengerId, RaceId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(ticket);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(ticket.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            ViewData["PassengerId"] = new SelectList(_context.Passengers, "Id", "Fullname", ticket.PassengerId);
            ViewData["RaceId"] = new SelectList(_context.Races, "Id", "NumberRace", ticket.RaceId);
            return RedirectToAction(nameof(Index));           
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Passenger)
                .Include(t => t.Race)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'Context.Tickets'  is null.");
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id)
        {
          return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private IQueryable<Ticket> SearchInName(IQueryable<Ticket> tickets, string searchName)
        {
            if (!String.IsNullOrEmpty(searchName))
            {
                tickets = tickets.Where(s => s.Passenger.Fullname.Contains(searchName));
            }
            return tickets;
        }
    }
}
