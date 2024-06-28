using System.Security.Claims;
using Opinion8.Models;

namespace Opinion8.Interfaces;

public interface IPollOptionService
{
    public PollOption? GetById(int id);

    public PollOption? GetById(ClaimsPrincipal? user, int id);

    public PollOption? GetByName(int pollId, string name);

    public IEnumerable<PollOption> SaveFromString(Poll poll, string? options);

    public void DeleteAllFromPoll(Poll poll);

    public void Save(PollOption pollOption);
}
