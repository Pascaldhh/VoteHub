using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Opinion8.Interfaces;
using Opinion8.Models;
using Opinion8.Services;

namespace Opinion8.Hubs;

public class PollHub(PollVoteService voteService, IPollOptionService optionService) : Hub
{
    [Authorize]
    public async Task PollVote(int? pollOptionId)
    {
        string? userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        PollOption? pollOption = optionService.GetById(pollOptionId ?? -1);
        if (pollOption == null || string.IsNullOrEmpty(userId))
            return;

        voteService.Vote(pollOption.Id, userId);
        PollService.SetUserVote(userId, pollOption.Poll);
        await PollUpdate(pollOption.Poll);
    }

    [Authorize]
    public async Task PollDelete(Poll poll) =>
        await Clients.All.SendAsync(nameof(PollDelete), poll);

    [Authorize]
    public async Task PollUpdate(Poll poll)
    {
        poll.Options = poll.Options.OrderBy(option => option.Id);

        await Clients.Caller.SendAsync(nameof(PollUpdate), poll);

        poll.HasVoted = null;
        await Clients.Others.SendAsync(nameof(PollUpdate), poll);
    }
}
