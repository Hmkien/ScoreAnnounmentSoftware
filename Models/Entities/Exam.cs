using System.ComponentModel.DataAnnotations;

namespace ScoreAnnouncementSoftware.Models.Entities
{
    public class Exam
    {
        [Key]
        public int ExamId { get; set; }
        [Display(Name = "Mã kỳ thi")]
        public string ExamCode { get; set; }
        [Display(Name = "Tên kỳ thi")]
        public string ExamName { get; set; }
        [Display(Name = "Ngày tạo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Display(Name = "Người tạo")]
        public string CreatePerson { get; set; } = "Admin";
        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }
        public bool? IsDelete { get; set; } = false;
        [Display(Name = "Trạng thái")]
        public string? Status { get; set; } = "Đang diễn ra";

    }
}