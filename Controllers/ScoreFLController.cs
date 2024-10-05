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
    public class ScoreFLController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScoreFLController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ScoreFL
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScoreFL.ToListAsync());
        }

        // GET: ScoreFL/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scoreFL = await _context.ScoreFL
                .FirstOrDefaultAsync(m => m.ScoreFLCode == id);
            if (scoreFL == null)
            {
                return NotFound();
            }

            return View(scoreFL);
        }

        // GET: ScoreFL/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScoreFL/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScoreFLCode,ExamCode,StudentCode,SpeakingScore,ReadingScore,WritingScore,ListeningScore,TotalScore,Result,Note")] ScoreFL scoreFL)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scoreFL);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scoreFL);
        }

        // GET: ScoreFL/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scoreFL = await _context.ScoreFL.FindAsync(id);
            if (scoreFL == null)
            {
                return NotFound();
            }
            return View(scoreFL);
        }

        // POST: ScoreFL/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScoreFLCode,ExamCode,StudentCode,SpeakingScore,ReadingScore,WritingScore,ListeningScore,TotalScore,Result,Note")] ScoreFL scoreFL)
        {
            if (id != scoreFL.ScoreFLCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scoreFL);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScoreFLExists(scoreFL.ScoreFLCode))
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
            return View(scoreFL);
        }

        // GET: ScoreFL/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scoreFL = await _context.ScoreFL
                .FirstOrDefaultAsync(m => m.ScoreFLCode == id);
            if (scoreFL == null)
            {
                return NotFound();
            }

            return View(scoreFL);
        }

        // POST: ScoreFL/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scoreFL = await _context.ScoreFL.FindAsync(id);
            if (scoreFL != null)
            {
                _context.ScoreFL.Remove(scoreFL);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScoreFLExists(int id)
        {
            return _context.ScoreFL.Any(e => e.ScoreFLCode == id);
        }
    }
}
