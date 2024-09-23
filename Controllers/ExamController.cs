using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScoreAnnouncementSoftware.Data;
using ScoreAnnouncementSoftware.Models.Entities;
using ScoreAnnouncementSoftware.Models.Process;
using ScoreAnnouncementSoftware.Models.ViewModels;

namespace ScoreAnnouncementSoftware.Controllers
{
    public class ExamController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ExcelProcess _excelProcess = new ExcelProcess();

        public ExamController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Exam
        public IActionResult Index()
        {
            var model = new ExamViewModel
            {
                Exams = _context.Exam.Where(e => e.IsDelete == false).ToList(),
                NewExam = new Exam()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exam newExam)
        {
            if (ModelState.IsValid)
            {
                newExam.CreateDate = DateTime.Now;
                _context.Add(newExam);
                await _context.SaveChangesAsync();
                TempData["Result"] = "Thêm mới thành công!";
                return RedirectToAction(nameof(Index));
            }
            TempData["Result"] = "Thêm mới thất bại, vui lòng điền thông tin hợp lệ!";
            return RedirectToAction(nameof(Index));
        }


        // GET: Exam/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exam.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            return View(exam);
        }

        // POST: Exam/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExamId,ExamCode,ExamName,CreateDate,CreatePerson,Note,IsDelete,Status")] Exam exam)
        {
            if (id != exam.ExamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamExists(exam.ExamId))
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
            return View(exam);
        }

        // GET: Exam/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exam
                .FirstOrDefaultAsync(m => m.ExamId == id);
            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }

        // POST: Exam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exam = await _context.Exam.FindAsync(id);
            if (exam != null)
            {
                exam.IsDelete = true;
                TempData["Result"] = "Đã xóa thành công!!";
            }
            else
            {
                TempData["Result"] = "Không tìm thấy bản ghi cần xóa,Vui lòng thử lại!!";
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExamExists(int id)
        {
            return _context.Exam.Any(e => e.ExamId == id);
        }
        public async Task<IActionResult> Config(int id)
        {
            var model = await _context.StudentExam.Where(e => e.ExamId == id).ToListAsync();

            var examModel = new ConfigVM
            {
                StudentExams = model,
                ExamId = id
            };

            return View(examModel);
        }
        [HttpPost]
        public async Task<IActionResult> Upload(int examid, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Result"] = "Vui lòng tải file lên !!!";
                return RedirectToAction("Config", new { id = examid });
            }

            var fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension.ToLower() != ".xlsx" && fileExtension.ToLower() != ".xls")
            {
                TempData["Result"] = "File tải lên không đúng định dạng, Vui lòng thử lại !!!";
                return RedirectToAction("Config", new { id = examid });
            }

            // Xử lý file upload ở đây

            return RedirectToAction("Config", new { id = examid });
        }


    }
}
