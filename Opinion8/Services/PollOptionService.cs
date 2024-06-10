using System.Security.Claims;
using Opinion8.Interfaces;
using Opinion8.Models;
using Opinion8.Repositories;

namespace Opinion8.Services;

public class PollOptionService(IPollOptionRepository repository) : IPollOptionService
{
    public PollOption? GetById(int id) => repository.GetById(id);

    public PollOption? GetById(ClaimsPrincipal? user, int id)
    {
        PollOption? pollOption = GetById(id);
        if (pollOption == null || user == null)
            return pollOption;

        PollService.SetUserVote(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, pollOption.Poll);

        return pollOption;
    }

    public PollOption? GetByName(int pollId, string name) => repository.GetByName(pollId, name);

    public IEnumerable<PollOption> SaveFromString(Poll poll, string? options)
    {
        if (string.IsNullOrEmpty(options))
            return [];

        List<PollOption> pollOptions = [];
        foreach (string optionStr in options.Split(","))
        {
            PollOption option = new() { Name = optionStr.Trim(), PollId = poll.Id };
            pollOptions.Add(option);
            Save(option);
        }

        return pollOptions;
    }

    public void DeleteAllFromPoll(Poll poll) => repository.DeleteAllFromPoll(poll);

    public void Save(PollOption pollOption) => repository.Save(pollOption);
}
