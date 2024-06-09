using Opinion8.Models;
using Opinion8.Repositories;

namespace Opinion8.Services;

public class PollService(PollRepository pollRepository)
{
    public Poll? GetById(int id) => pollRepository.GetById(id);

    public IEnumerable<Poll> GetAll() => pollRepository.GetAll();

    public void Save(Poll poll) => pollRepository.Save(poll);

    public void Delete(Poll poll) => pollRepository.Delete(poll);
}
