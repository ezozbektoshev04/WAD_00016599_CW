namespace FeedbackSystem.Models
{
    public class FeedbackDTO
    {
        //00016599
        public int FeedbackId { get; set; }          
        public int UserId { get; set; }               
        public string FeedbackTitle { get; set; }     
        public string FeedbackDescription { get; set; } 
        public DateTime FeedbackCreatedDate { get; set; }
        public string UserName { get; set; }
    }
    public class CreateFeedbackDTO
    {
        //00016599
        public int UserId { get; set; }
        public string FeedbackTitle { get; set; }
        public string FeedbackDescription { get; set; }
    }
}
