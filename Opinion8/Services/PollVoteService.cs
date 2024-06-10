using Opinion8.Data;
using Opinion8.Models;
using Opinion8.Repositories;

namespace Opinion8.Services;

public class PollVoteService(PollVoteRepository repository)
{
    public void Vote(int pollOptionId, string userId) =>
        repository.Save(new PollVote { PollOptionId = pollOptionId, UserId = userId });
}
