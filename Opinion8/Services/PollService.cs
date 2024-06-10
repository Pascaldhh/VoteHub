using System.Security.Claims;
using Opinion8.Interfaces;
using Opinion8.Models;
using Opinion8.Repositories;

namespace Opinion8.Services;

public class PollService(IPollRepository pollRepository)
{
    public Poll? GetById(int id) => pollRepository.GetById(id);

    public IEnumerable<PollVote> GetAllVoters(Poll poll) =>
        poll.Options.SelectMany(option => option.Votes).Distinct();

    public IEnumerable<Poll> GetAll() => pollRepository.GetAll();

    public IEnumerable<Poll> GetAll(ClaimsPrincipal? user)
    {
        List<Poll> polls = pollRepository.GetAll().ToList();
        foreach (Poll poll in polls)
            SetUserVote(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value, poll);
        return polls;
    }

    public void Save(Poll poll) => pollRepository.Save(poll);

    public void Delete(Poll poll) => pollRepository.Delete(poll);

    public static void SetUserVote(string? userId, Poll poll)
    {
        poll.HasVoted = poll.Options.Any(option =>
            option.Votes.Any(vote => vote.UserId.Equals(userId))
        );
    }
}
