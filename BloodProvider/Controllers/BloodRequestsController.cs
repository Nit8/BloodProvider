using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BloodProvider.Core.Entities;
using BloodProvider.Infrastructure.Data;

namespace BloodProvider.Web.Controllers
{
    public class BloodRequestsController : Controller
    {
        private readonly AppDbContext _context;

        public BloodRequestsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: BloodRequests
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.BloodRequests.Include(b => b.Hospital);
            return View(await appDbContext.ToListAsync());
        }

        // GET: BloodRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bloodRequest = await _context.BloodRequests
                .Include(b => b.Hospital)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bloodRequest == null)
            {
                return NotFound();
            }

            return View(bloodRequest);
        }

        // GET: BloodRequests/Create
        public IActionResult Create()
        {
            ViewData["HospitalId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: BloodRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BloodType,Quantity,HospitalId,RequestDate,IsUrgent,Status")] BloodRequest bloodRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bloodRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HospitalId"] = new SelectList(_context.Users, "Id", "Id", bloodRequest.HospitalId);
            return View(bloodRequest);
        }

        // GET: BloodRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bloodRequest = await _context.BloodRequests.FindAsync(id);
            if (bloodRequest == null)
            {
                return NotFound();
            }
            ViewData["HospitalId"] = new SelectList(_context.Users, "Id", "Id", bloodRequest.HospitalId);
            return View(bloodRequest);
        }

        // POST: BloodRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BloodType,Quantity,HospitalId,RequestDate,IsUrgent,Status")] BloodRequest bloodRequest)
        {
            if (id != bloodRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bloodRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BloodRequestExists(bloodRequest.Id))
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
            ViewData["HospitalId"] = new SelectList(_context.Users, "Id", "Id", bloodRequest.HospitalId);
            return View(bloodRequest);
        }

        // GET: BloodRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bloodRequest = await _context.BloodRequests
                .Include(b => b.Hospital)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bloodRequest == null)
            {
                return NotFound();
            }

            return View(bloodRequest);
        }

        // POST: BloodRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bloodRequest = await _context.BloodRequests.FindAsync(id);
            if (bloodRequest != null)
            {
                _context.BloodRequests.Remove(bloodRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BloodRequestExists(int id)
        {
            return _context.BloodRequests.Any(e => e.Id == id);
        }
    }
}
