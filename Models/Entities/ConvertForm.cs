using System.ComponentModel.DataAnnotations;

namespace ScoreAnnouncementSoftware.Models.Entities
{
    public class ConvertForm
    {
        [Key]
        public int ConvertFormId { get; set; }
        public string StudentCode { get; set; }
        public string CertificateType { get; set; }
        public string CertificateName { get; set; }
        public DateTime SendDate { get; set; } = DateTime.Now;
        public string fileDocx { get; set; }
        public string? Note { get; set; }
    }
}