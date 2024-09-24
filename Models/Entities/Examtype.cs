using System.ComponentModel.DataAnnotations;

namespace ScoreAnnouncementSoftware.Models.Entities
{
    public class ExamType
    {
        [Key]
        public string ExamTypeId { get; set; }
        public string ExamTypeName { get; set; }
        public virtual ICollection<Exam> Exam { get; set; }
    }
}