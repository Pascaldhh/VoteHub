using Microsoft.EntityFrameworkCore;
using Opinion8.Data;
using Opinion8.Interfaces;
using Opinion8.Models;

namespace Opinion8.Repositories;

public class PollRepository(ApplicationDbContext applicationDbContext) : IPollRepository
{
    public Poll? GetById(int id) =>
        applicationDbContext
            .Polls.Include(poll => poll.Options)
            .ThenInclude(option => option.Votes)
            .FirstOrDefault(poll => poll.Id == id);

    public IEnumerable<Poll> GetAll() =>
        applicationDbContext
            .Polls.Include(poll => poll.Options)
            .ThenInclude(option => option.Votes);

    public void Save(Poll poll)
    {
        applicationDbContext.Entry(poll).State =
            poll.Id == 0 ? EntityState.Added : EntityState.Modified;
        applicationDbContext.SaveChanges();
    }

    public void Delete(Poll poll)
    {
        applicationDbContext.Polls.Remove(poll);
        applicationDbContext.SaveChanges();
    }
}
