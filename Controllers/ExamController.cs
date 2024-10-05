using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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
                Exams = _context.Exam.Include(e => e.ExamType).Where(e => e.IsDelete == false).ToList(),
                NewExam = new Exam()
            };
            ViewData["ExamType"] = new SelectList(_context.ExamType, "ExamTypeId", "ExamTypeName");

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
            ViewData["ExamType"] = new SelectList(_context.ExamType, "ExamTypeId", "ExamTypeName", exam.ExamTypeId);
            return View(exam);
        }

        // POST: Exam/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExamId,ExamCode,ExamName,CreateDate,CreatePerson,ExamTypeId,Note,IsDelete,Status")] Exam exam)
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
                    TempData["Result"] = "Đã thay đổi thành công!!";

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

            TempData["Result"] = "Thất bại!!!";
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
                ExamId = id,
            };
            ViewBag.ExamName = _context.Exam.Where(e => e.ExamId == id).Select(e => e.ExamType.ExamTypeName).FirstOrDefault();

            return View(examModel);
        }
        public async Task<IActionResult> DeleteAll()
        {
            var model = await _context.Student.ToListAsync();
            var modal = await _context.ScoreFL.ToListAsync();
            _context.ScoreFL.RemoveRange(modal);
            _context.Student.RemoveRange(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Upload(int examid, IFormFile file)
        {
            // Kiểm tra file null hoặc không hợp lệ
            if (file == null || file.Length == 0 || !(Path.GetExtension(file.FileName).ToLower() == ".xlsx" || Path.GetExtension(file.FileName).ToLower() == ".xls"))
            {
                TempData["Message"] = file == null || file.Length == 0
                    ? "Vui lòng tải file lên !!!"
                    : "File tải lên không đúng định dạng, vui lòng thử lại !!!";
                return RedirectToAction("Config", new { id = examid });
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var exam = await _context.Exam.FirstOrDefaultAsync(e => e.ExamId == examid);
            if (exam == null)
            {
                TempData["Message"] = "Kỳ thi không tồn tại!";
                return RedirectToAction("Config", new { id = examid });
            }
            if (exam.Status == "Đã kết thúc")
            {
                TempData["Message"] = "Kỳ thi đã kết thúc. Vui lòng liên hệ quản trị viên để biết thêm chi tiết!";
                return RedirectToAction("Config", new { id = examid });
            }

            try
            {
                var tempFilePath = Path.GetTempFileName();
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var result = await ImportDataFromExcel(examid, tempFilePath);
                TempData["Message"] = result;
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Đã xảy ra lỗi: " + ex.Message + ". Vui lòng thử lại sau!";
            }

            return RedirectToAction("Config", new { id = examid });
        }

        private async Task<string> ImportDataFromExcel(int examId, string fileLocation)
        {
            string messageResult = await CheckDataFromExcel(fileLocation, examId);

            string examType = await _context.Exam
                .Where(e => e.ExamId == examId)
                .Select(e => e.ExamTypeId)
                .FirstOrDefaultAsync();

            var dataFromExcel = _excelProcess.ExcelToDataTable(fileLocation);
            if (dataFromExcel == null || dataFromExcel.Rows.Count == 0)
            {
                return "Dữ liệu file Excel rỗng hoặc không đọc được!";
            }

            var dataRows = dataFromExcel.AsEnumerable();

            if (examType == "1")
            {
                var existingStudentCodes = await _context.Student
                    .Select(m => m.StudentCode)
                    .ToListAsync();

                try
                {
                    var newStudents = dataRows
                        .Where(row => !existingStudentCodes.Contains(row.Field<string>(1)))
                        .Select(row => new Student
                        {
                            StudentCode = row.Field<string>(1),
                            FirstName = row.Field<string>(3),
                            LastName = row.Field<string>(2),
                            Course = row.Field<string>(11)
                        })
                        .ToList();

                    if (newStudents.Any())
                    {
                        await _context.Student.AddRangeAsync(newStudents);
                    }
                    var scoreFLs = dataRows.Select(row => new ScoreFL
                    {
                        StudentCode = row.Field<string>(1),
                        ListeningScore = row.Field<string>(4),
                        ReadingScore = row.Field<string>(5),
                        WritingScore = row.Field<string>(6),
                        SpeakingScore = row.Field<string>(7),
                        TotalScore = row.Field<string>(8),
                        Result = row.Field<string>(9),
                        Note = row.Field<string>(10),
                        ExamId = examId
                    }).ToList();

                    await _context.ScoreFL.AddRangeAsync(scoreFLs);
                    var exam = _context.Exam.Where(e => e.ExamId == examId).FirstOrDefault();
                    exam.Status = "Đã kết thúc";
                    await _context.BulkSaveChangesAsync();

                    messageResult = $"Import thành công {dataFromExcel.Rows.Count} sinh viên.";
                }
                catch (Exception ex)
                {
                    messageResult = "Dữ liệu bị lỗi, vui lòng kiểm tra lại dữ liệu file excel! " + ex.Message;
                }
            }

            else if (examType == "0")
            {
                var existingStudentCodes = await _context.Student
                    .Select(m => m.StudentCode)
                    .ToListAsync();

                try
                {
                    var newStudents = dataRows
                        .Where(row => !existingStudentCodes.Contains(row.Field<string>(1)))
                        .Select(row => new Student
                        {
                            StudentCode = row.Field<string>(1),
                            FirstName = row.Field<string>(3),
                            LastName = row.Field<string>(2),

                        })
                        .ToList();

                    if (newStudents.Any())
                    {
                        await _context.Student.AddRangeAsync(newStudents);
                    }
                    var scoreFLs = dataRows.Select(row => new ScoreFL
                    {
                        StudentCode = row.Field<string>(1),
                        ListeningScore = row.Field<string>(4),
                        ReadingScore = row.Field<string>(5),
                        WritingScore = row.Field<string>(6),
                        SpeakingScore = row.Field<string>(7),
                        TotalScore = row.Field<string>(8),
                        Result = row.Field<string>(9),
                        Note = row.Field<string>(10),
                        ExamId = examId
                    }).ToList();

                    await _context.ScoreFL.AddRangeAsync(scoreFLs);
                    var exam = _context.Exam.Where(e => e.ExamId == examId).FirstOrDefault();
                    exam.Status = "Đã kết thúc";
                    await _context.BulkSaveChangesAsync();

                    messageResult = $"Import thành công {dataFromExcel.Rows.Count} sinh viên.";
                }
                catch (Exception ex)
                {
                    messageResult = "Dữ liệu bị lỗi, vui lòng kiểm tra lại dữ liệu file excel! " + ex.Message;
                }
            }

            else if (examType == "2")
            {
                var existingStudentCodes = await _context.Student
                      .Select(m => m.StudentCode)
                      .ToListAsync();

                try
                {
                    var newStudents = dataRows
                        .Where(row => !existingStudentCodes.Contains(row.Field<string>(1)))
                        .Select(row => new Student
                        {
                            StudentCode = row.Field<string>(1),
                            FirstName = row.Field<string>(3),
                            LastName = row.Field<string>(2),
                        })
                        .ToList();

                    if (newStudents.Any())
                    {
                        await _context.Student.AddRangeAsync(newStudents);
                    }
                    var scoreITs = dataRows.Select(row => new ScoreIT
                    {
                        StudentCode = row.Field<string>(1),
                        PracticalScore = row.Field<string>(5),
                        TheoryScore = row.Field<string>(4),
                        TotalScore = row.Field<string>(6),
                        Result = row.Field<string>(7),
                        Note = row.Field<string>(8),
                        examId = examId
                    }).ToList();
                    await _context.ScoreIT.AddRangeAsync(scoreITs);
                    var exam = _context.Exam.Where(e => e.ExamId == examId).FirstOrDefault();
                    exam.Status = "Đã kết thúc";
                    await _context.BulkSaveChangesAsync();
                    messageResult = $"Import thành công {dataFromExcel.Rows.Count} sinh viên.";
                }
                catch (Exception ex)
                {
                    messageResult = "Dữ liệu bị lỗi, vui lòng kiểm tra lại dữ liệu file excel! " + ex.Message;
                }
            }

            else if (examType == "3")

            {

                var existingStudentCodes = await _context.ITStudent
                     .Select(m => m.IdentityNumber)
                     .ToListAsync();

                try
                {
                    var newITStudents = dataRows
                        .Where(row => !existingStudentCodes.Contains(row.Field<string>(2)))
                        .Select(row => new ITStudent
                        {
                            IdentificationCode = row.Field<string>(1),
                            IdentityNumber = row.Field<string>(2),
                            LastName = row.Field<string>(3),
                            FirstName = row.Field<string>(4),
                            BirthDay = row.Field<string>(5),
                            Gender = row.Field<string>(6),
                            national = row.Field<string>(7),
                            Address = row.Field<string>(8),
                            ExamId = examId

                        })
                        .ToList();

                    if (newITStudents.Any())
                    {
                        await _context.ITStudent.AddRangeAsync(newITStudents);
                    }
                    var scoreITs = dataRows.Select(row => new ScoreIT
                    {
                        IdentityNumber = row.Field<string>(2),
                        TheoryScore = row.Field<string>(9),
                        PracticalScore = row.Field<string>(10),
                        Result = row.Field<string>(11),
                        Note = row.Field<string>(12),
                        examId = examId
                    }).ToList();

                    await _context.ScoreIT.AddRangeAsync(scoreITs);
                    var exam = _context.Exam.Where(e => e.ExamId == examId).FirstOrDefault();
                    exam.Status = "Đã hoàn thành";
                    await _context.BulkSaveChangesAsync();

                    messageResult = $"Import thành công {dataFromExcel.Rows.Count} sinh viên.";
                }
                catch (Exception ex)
                {
                    messageResult = "Dữ liệu bị lỗi, vui lòng kiểm tra lại dữ liệu file excel! " + ex.Message;
                }
            }

            else
            {
                messageResult = "Kiểm tra lại dữ liệu dòng: " + messageResult;
            }
            return messageResult;

        }


        private async Task<string> CheckDataFromExcel(string fileLocation, int examid)
        {
            string messageResult = "";
            int examType = Convert.ToInt32(await _context.Exam.Where(e => e.ExamId == examid).Select(e => e.ExamTypeId).FirstOrDefaultAsync());
            var dataFromExcel = _excelProcess.ExcelToDataTable(fileLocation);

            for (int i = 0; i < dataFromExcel.Rows.Count; i++)
            {
                try
                {
                    if (examType == 1)
                    {
                        var stdExam = new StudentExam();
                        stdExam.StudentCode = dataFromExcel.Rows[i][1].ToString();
                        stdExam.ExamId = examid;
                        stdExam.Note = dataFromExcel.Rows[i][9].ToString();
                        var student = new Student();
                        student.StudentCode = dataFromExcel.Rows[i][1].ToString();
                        student.LastName = dataFromExcel.Rows[i][2].ToString();
                        student.FirstName = dataFromExcel.Rows[i][3].ToString();
                        student.Course = dataFromExcel.Rows[i][11].ToString();
                        var FLScore = new ScoreFL();
                        FLScore.ListeningScore = dataFromExcel.Rows[i][4].ToString();
                        FLScore.ReadingScore = dataFromExcel.Rows[i][5].ToString();
                        FLScore.SpeakingScore = dataFromExcel.Rows[i][6].ToString();
                        FLScore.WritingScore = dataFromExcel.Rows[i][7].ToString();
                        FLScore.TotalScore = dataFromExcel.Rows[i][8].ToString();
                        FLScore.Result = dataFromExcel.Rows[i][9].ToString();
                        FLScore.Note = dataFromExcel.Rows[i][10].ToString();
                    }
                    else if (examType == 0)
                    {
                        var stdExam = new StudentExam();
                        stdExam.StudentCode = dataFromExcel.Rows[i][1].ToString();
                        stdExam.ExamId = examid;
                        stdExam.Note = dataFromExcel.Rows[i][9].ToString();
                        var student = new Student();
                        student.StudentCode = dataFromExcel.Rows[i][1].ToString();
                        // student.FullName = dataFromExcel.Rows[i][2].ToString();
                        var FLScore = new ScoreFL();
                        FLScore.ListeningScore = dataFromExcel.Rows[i][3].ToString();
                        FLScore.ReadingScore = dataFromExcel.Rows[i][4].ToString();
                        FLScore.WritingScore = dataFromExcel.Rows[i][5].ToString();
                        FLScore.SpeakingScore = dataFromExcel.Rows[i][6].ToString();
                        FLScore.TotalScore = dataFromExcel.Rows[i][7].ToString();
                        FLScore.Result = dataFromExcel.Rows[i][8].ToString();
                        FLScore.Note = dataFromExcel.Rows[i][9].ToString();
                    }
                    else if (examType == 2)
                    {
                        var stdExam = new StudentExam();
                        stdExam.StudentCode = dataFromExcel.Rows[i][1].ToString();
                        stdExam.ExamId = examid;
                        stdExam.Note = dataFromExcel.Rows[i][7].ToString();
                        var student = new Student();
                        student.StudentCode = dataFromExcel.Rows[i][1].ToString();
                        // student.FullName = dataFromExcel.Rows[i][2].ToString();
                        var FLScore = new ScoreIT();
                        FLScore.TheoryScore = dataFromExcel.Rows[i][3].ToString();
                        FLScore.PracticalScore = dataFromExcel.Rows[i][4].ToString();
                        FLScore.TotalScore = dataFromExcel.Rows[i][5].ToString();
                        FLScore.Result = dataFromExcel.Rows[i][6].ToString();
                        FLScore.Note = dataFromExcel.Rows[i][7].ToString();
                    }
                    else if (examType == 3)
                    {
                        var stdExam = new ITStudent();
                        stdExam.IdentificationCode = dataFromExcel.Rows[i][1].ToString();
                        stdExam.ExamId = examid;
                        stdExam.IdentityNumber = dataFromExcel.Rows[i][2].ToString();
                        // stdExam.FullName = dataFromExcel.Rows[i][3].ToString();
                        stdExam.BirthDay = dataFromExcel.Rows[i][4].ToString();
                        stdExam.Gender = dataFromExcel.Rows[i][5].ToString();
                        stdExam.Address = dataFromExcel.Rows[i][6].ToString();
                        stdExam.Note = dataFromExcel.Rows[i][11].ToString();
                        var FLScore = new ScoreIT();
                        FLScore.TheoryScore = dataFromExcel.Rows[i][8].ToString();
                        FLScore.PracticalScore = dataFromExcel.Rows[i][9].ToString();
                        FLScore.Result = dataFromExcel.Rows[i][10].ToString();
                        FLScore.Note = dataFromExcel.Rows[i][11].ToString();
                    }
                }
                catch
                {
                    messageResult += (i + 1) + "; ";
                }
            }

            return messageResult;
        }
    }
}

