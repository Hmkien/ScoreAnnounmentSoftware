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
    public class ConvertFormController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConvertFormController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ConvertForm
        public async Task<IActionResult> Index()
        {
            return View(await _context.ConvertForms.ToListAsync());
        }

        // GET: ConvertForm/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var convertForm = await _context.ConvertForms
                .FirstOrDefaultAsync(m => m.ConvertFormId == id);
            if (convertForm == null)
            {
                return NotFound();
            }

            return View(convertForm);
        }

        // GET: ConvertForm/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ConvertForm/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConvertFormId,StudentCode,CertificateType,CertificateName,SendDate,fileDocx,Note")] ConvertForm convertForm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(convertForm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(convertForm);
        }

        // GET: ConvertForm/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var convertForm = await _context.ConvertForms.FindAsync(id);
            if (convertForm == null)
            {
                return NotFound();
            }
            return View(convertForm);
        }

        // POST: ConvertForm/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConvertFormId,StudentCode,CertificateType,CertificateName,SendDate,fileDocx,Note")] ConvertForm convertForm)
        {
            if (id != convertForm.ConvertFormId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(convertForm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConvertFormExists(convertForm.ConvertFormId))
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
            return View(convertForm);
        }

        // GET: ConvertForm/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var convertForm = await _context.ConvertForms
                .FirstOrDefaultAsync(m => m.ConvertFormId == id);
            if (convertForm == null)
            {
                return NotFound();
            }

            return View(convertForm);
        }

        // POST: ConvertForm/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var convertForm = await _context.ConvertForms.FindAsync(id);
            if (convertForm != null)
            {
                _context.ConvertForms.Remove(convertForm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConvertFormExists(int id)
        {
            return _context.ConvertForms.Any(e => e.ConvertFormId == id);
        }
        [HttpPost]
        public async Task<IActionResult> Upload(int examid, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Result"] = "Vui lòng tải file lên !!!";
                return RedirectToAction("Index");
            }

            var fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension.ToLower() != ".xlsx" && fileExtension.ToLower() != ".xls")
            {
                TempData["Result"] = "File tải lên không đúng định dạng, Vui lòng thử lại !!!";
                return RedirectToAction("Index");
            }

            var fileName = file.FileName;
            var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot//Upload/Excels", fileName);
            var fileLocation = new FileInfo(filePath).ToString();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                TempData["Result"] = "Upload kết quả kì thi thành công,Vui lòng kiểm tra kết quả";
            }

            return RedirectToAction("Index");
        }

    }
}
