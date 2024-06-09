using System.ComponentModel.DataAnnotations;

namespace Opinion8.Models;

public class Poll
{
    public int Id { get; set; }

    [MinLength(8), MaxLength(255)]
    public string Question { get; set; } = string.Empty;
    public IEnumerable<PollOption> Options { get; set; }
    public int Voters { get; set; }
}
