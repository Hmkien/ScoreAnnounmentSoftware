using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScoreAnnouncementSoftware.Models.Entities
{
    public class ITStudent
    {
        [Key]
        public Guid ITStudentId { get; set; }

        [Display(Name = "Student Code")]
        public string StudentCode { get; set; }
        public string IdentificationCode { get; set; }
        [ForeignKey("StudentCode")]
        public Student? Student { get; set; }
        public string Note { get; set; }

    }
}
