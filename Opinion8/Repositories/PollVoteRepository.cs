using Microsoft.EntityFrameworkCore;
using Opinion8.Data;
using Opinion8.Models;

namespace Opinion8.Repositories;

public class PollVoteRepository(ApplicationDbContext applicationDbContext)
{
    public void Save(PollVote pollVote)
    {
        applicationDbContext.Entry(pollVote).State =
            pollVote.Id == 0 ? EntityState.Added : EntityState.Modified;
        applicationDbContext.SaveChanges();
    }
}
