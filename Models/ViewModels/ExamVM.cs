using ScoreAnnouncementSoftware.Models.Entities;

namespace ScoreAnnouncementSoftware.Models.ViewModels;
public class ExamViewModel
{
    public IEnumerable<Exam> Exams { get; set; }
    public Exam NewExam { get; set; }
}