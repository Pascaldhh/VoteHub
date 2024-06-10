namespace Opinion8.Models;

public class PollVote
{
    public int Id { get; set; }
    public int PollOptionId { get; set; }
    public PollOption PollOption { get; set; }
    public string UserId { get; set; }
}
