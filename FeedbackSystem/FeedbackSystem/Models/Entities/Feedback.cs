namespace FeedbackSystem.Models.Entities
{
    public class Feedback
    {
        //00016599
        public int FeedbackId { get; set; }
        public int UserId { get; set; }
        public string FeedbackTitle { get; set; }
        public string FeedbackDescription { get; set; }
        public DateTime FeedbackCreatedDate { get; set; }
        public Users User { get; set; }

    }
}
