namespace Backend.Models
{
    public class Poll
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TeamId { get; set; }
        public DateTime VoteDate { get; set; }

        public User? User { get; set; }
        public Team? Team { get; set; }
    }
}
