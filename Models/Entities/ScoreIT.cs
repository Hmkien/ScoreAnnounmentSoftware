using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreAnnouncementSoftware.Models.Entities
{
    public class ScoreIT
    {
        [Key]
        public int ScoreITCode { get; set; }
        [ForeignKey("StudentCode")]
        public Guid StudentCode { get; set; }
        [ForeignKey("ExamCode")]
        public int ExamCode { get; set; }
        [Display(Name = "Điểm thực hành")]
        public string PracticalScore { get; set; }
        [Display(Name = "Điểm lý thuyết")]
        public string TheoryScore { get; set; }
        [Display(Name = "Tổng điểm")]
        public string TotalScore { get; set; }


    }
}