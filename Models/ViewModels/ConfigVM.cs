using ScoreAnnouncementSoftware.Models.Entities;

namespace ScoreAnnouncementSoftware.Models.ViewModels
{
    public class ConfigVM
    {
        public int ExamId { get; set; }
        public List<StudentExam> StudentExams { get; set; }
    }

}