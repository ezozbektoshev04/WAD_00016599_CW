using System.ComponentModel.DataAnnotations;

namespace FeedbackSystem.Models.Entities
{
    public class Users
    {
        //00016599
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }

    }
}
