using System;
using System.ComponentModel.DataAnnotations;

namespace ScoreAnnouncementSoftware.Models.Entities
{
    public class Student
    {
        [Key]
        [Required]
        [Display(Name = "Mã sinh viên")]
        public string StudentCode { get; set; }

        [Required]
        [Display(Name = "Tên")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Họ")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name = "Miễn chuẩn đầu ra")]
        public int Note { get; set; } = 0;

        [Required]
        [Display(Name = "Khóa học")]
        public string Course { get; set; }

        [Required]
        [Display(Name = "Khoa")]
        public string Faculty { get; set; }

        [Display(Name = "Hoạt động")]
        public bool? IsActive { get; set; }

        [Display(Name = "Đã xóa")]
        public bool? IsDelete { get; set; }
    }
}
