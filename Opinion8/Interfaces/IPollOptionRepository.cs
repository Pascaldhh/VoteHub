using Opinion8.Models;

namespace Opinion8.Interfaces;

public interface IPollOptionRepository
{
    public PollOption? GetById(int id);

    public PollOption? GetByName(int pollId, string name);

    public void Save(PollOption pollOption);

    public void DeleteAllFromPoll(Poll poll);
}
