using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace Opinion8.Models;

public class Poll
{
    public int Id { get; set; }

    [MinLength(8), MaxLength(255)]
    public string Question { get; set; }

    public IEnumerable<PollOption> Options { get; set; } = new List<PollOption>();

    [NotMapped]
    public int VoteCount => Options.Sum(option => option.Votes.Count());

    [NotMapped]
    public bool? HasVoted { get; set; }
}
