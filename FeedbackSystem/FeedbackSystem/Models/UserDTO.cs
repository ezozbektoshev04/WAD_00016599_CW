namespace FeedbackSystem.Models
{
    public class UserDTO
    {
        //00016599
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }

    public class CreateUserDTO
    {
        //00016599
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}
