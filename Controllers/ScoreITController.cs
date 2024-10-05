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
    public class ScoreITController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScoreITController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ScoreIT
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScoreIT.ToListAsync());
        }

        // GET: ScoreIT/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scoreIT = await _context.ScoreIT
                .FirstOrDefaultAsync(m => m.ScoreITCode == id);
            if (scoreIT == null)
            {
                return NotFound();
            }

            return View(scoreIT);
        }

        // GET: ScoreIT/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScoreIT/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScoreITCode,StudentCode,ExamCode,PracticalScore,WordScore,ExcelScore,PowerPointScore,TheoryScore,TotalScore")] ScoreIT scoreIT)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scoreIT);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scoreIT);
        }

        // GET: ScoreIT/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scoreIT = await _context.ScoreIT.FindAsync(id);
            if (scoreIT == null)
            {
                return NotFound();
            }
            return View(scoreIT);
        }

        // POST: ScoreIT/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScoreITCode,StudentCode,ExamCode,PracticalScore,WordScore,ExcelScore,PowerPointScore,TheoryScore,TotalScore")] ScoreIT scoreIT)
        {
            if (id != scoreIT.ScoreITCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scoreIT);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScoreITExists(scoreIT.ScoreITCode))
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
            return View(scoreIT);
        }

        // GET: ScoreIT/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scoreIT = await _context.ScoreIT
                .FirstOrDefaultAsync(m => m.ScoreITCode == id);
            if (scoreIT == null)
            {
                return NotFound();
            }

            return View(scoreIT);
        }

        // POST: ScoreIT/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scoreIT = await _context.ScoreIT.FindAsync(id);
            if (scoreIT != null)
            {
                _context.ScoreIT.Remove(scoreIT);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScoreITExists(int id)
        {
            return _context.ScoreIT.Any(e => e.ScoreITCode == id);
        }
    }
}
