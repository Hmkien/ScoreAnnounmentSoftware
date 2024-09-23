using ScoreAnnouncementSoftware.Models.Entities;

namespace ScoreAnnouncementSoftware.Models.ViewModels;
public class ExamViewModel
{
    public int ExamId { get; set; }
    public IEnumerable<Exam> Exams { get; set; }
    public Exam NewExam { get; set; }
}