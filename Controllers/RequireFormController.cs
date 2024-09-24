using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScoreAnnouncementSoftware.Data;
using ScoreAnnouncementSoftware.Models.Entities;

namespace ScoreAnnouncementSoftware.Controllers
{
    public class RequireFormController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequireFormController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RequireForm
        public async Task<IActionResult> Index()
        {
            return View(await _context.RequireForms.ToListAsync());
        }

        // GET: RequireForm/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requireForm = await _context.RequireForms
                .FirstOrDefaultAsync(m => m.RequireFormCode == id);
            if (requireForm == null)
            {
                return NotFound();
            }

            return View(requireForm);
        }

        // GET: RequireForm/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RequireForm/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequireFormCode,StudenCode,FileDocx,Note,Status")] RequireForm requireForm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requireForm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(requireForm);
        }

        // GET: RequireForm/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requireForm = await _context.RequireForms.FindAsync(id);
            if (requireForm == null)
            {
                return NotFound();
            }
            return View(requireForm);
        }

        // POST: RequireForm/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequireFormCode,StudenCode,FileDocx,Note,Status")] RequireForm requireForm)
        {
            if (id != requireForm.RequireFormCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requireForm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequireFormExists(requireForm.RequireFormCode))
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
            return View(requireForm);
        }

        // GET: RequireForm/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requireForm = await _context.RequireForms
                .FirstOrDefaultAsync(m => m.RequireFormCode == id);
            if (requireForm == null)
            {
                return NotFound();
            }

            return View(requireForm);
        }

        // POST: RequireForm/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requireForm = await _context.RequireForms.FindAsync(id);
            if (requireForm != null)
            {
                _context.RequireForms.Remove(requireForm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequireFormExists(int id)
        {
            return _context.RequireForms.Any(e => e.RequireFormCode == id);
        }
    }
}
