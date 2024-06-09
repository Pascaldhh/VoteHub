using Microsoft.AspNetCore.SignalR;
using Opinion8.Models;
using Opinion8.Services;

namespace Opinion8.Hubs;

public class PollHub(PollService pollService) : Hub
{
    public async Task PollVote(int? id)
    {
        if (id == null)
            return;

        Poll? poll = pollService.GetById(id.Value);

        if (poll == null)
            return;

        poll.Voters++;

        await PollUpdate(poll);
    }

    public async Task PollDelete(Poll poll) =>
        await Clients.All.SendAsync(nameof(PollDelete), poll);

    public async Task PollUpdate(Poll poll)
    {
        pollService.Save(poll);
        await Clients.All.SendAsync(nameof(PollUpdate), poll);
    }
}
