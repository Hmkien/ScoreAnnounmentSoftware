using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreAnnouncementSoftware.Models.Entities
{
    public class StudentExam
    {
        [Key]
        public Guid StudentExamId { get; set; }

        [Required]
        [Display(Name = "Số báo danh")]
        public string IdentificationNumber { get; set; }

        [Required]
        public bool? IsActive { get; set; } = true;

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
        [ForeignKey("ExamId")]
        public int ExamId { get; set; }

        [ForeignKey("Student")]
        public string StudentCode { get; set; }
        public Student? Student { get; set; }
    }
}
