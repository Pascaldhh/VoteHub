using System.ComponentModel.DataAnnotations;

namespace Opinion8.Models;

public class PollOption
{
    public int Id { get; set; }

    [MaxLength(255)]
    public string Name { get; set; }

    public int PollId { get; set; }
    public Poll Poll { get; set; }
}
