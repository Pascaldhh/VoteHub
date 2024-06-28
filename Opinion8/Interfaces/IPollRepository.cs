using Opinion8.Models;

namespace Opinion8.Interfaces;

public interface IPollRepository
{
    public Poll? GetById(int id);

    public IEnumerable<Poll> GetAll();

    public void Save(Poll poll);

    public void Delete(Poll poll);
}
