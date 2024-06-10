using Microsoft.EntityFrameworkCore;
using Opinion8.Data;
using Opinion8.Interfaces;
using Opinion8.Models;

namespace Opinion8.Repositories;

public class PollOptionRepository(ApplicationDbContext applicationDbContext) : IPollOptionRepository
{
    public PollOption? GetById(int id) =>
        applicationDbContext
            .PollOptions.Include(option => option.Poll)
            .ThenInclude(poll => poll.Options)
            .ThenInclude(options => options.Votes)
            .FirstOrDefault(poll => poll.Id == id);

    public PollOption? GetByName(int pollId, string name) =>
        applicationDbContext.PollOptions.FirstOrDefault(pOption =>
            pOption.Id == pollId && pOption.Name.Equals(name)
        );

    public void Save(PollOption pollOption)
    {
        applicationDbContext.Entry(pollOption).State =
            pollOption.Id == 0 ? EntityState.Added : EntityState.Modified;
        applicationDbContext.SaveChanges();
    }

    public void DeleteAllFromPoll(Poll poll)
    {
        applicationDbContext.PollOptions.RemoveRange(poll.Options);
        applicationDbContext.SaveChanges();
    }
}
