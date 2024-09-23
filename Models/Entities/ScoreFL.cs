using System.ComponentModel.DataAnnotations;

namespace ScoreAnnouncementSoftware.Models.Entities
{
    public class ScoreFL
    {
        [Key]
        public int ScoreFLCode { get; set; }
        public int ExamCode { get; set; }
        public Guid StudentCode { get; set; }
        public string SpeakingScore { get; set; }
        public string ReadingScore { get; set; }
        public string WritingScore { get; set; }
        public string ListeningScore { get; set; }
        public string TotalScore { get; set; }



    }
}