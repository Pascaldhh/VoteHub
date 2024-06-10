using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Opinion8.Hubs;
using Opinion8.Interfaces;
using Opinion8.Models;
using Opinion8.Services;

namespace Opinion8.Controllers;

[Route("/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController(
    IHubContext<PollHub> pollContext,
    PollService pollService,
    IPollOptionService pollOptionService
) : Controller
{
    public IActionResult Index()
    {
        return View(pollService.GetAll());
    }

    [HttpGet("[action]")]
    public IActionResult Create()
    {
        ViewData["Title"] = "Create poll - Admin Page";
        return View("CreateEdit");
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Create(Poll poll)
    {
        pollService.Save(poll);

        string? options = Request.Form["Options"];
        pollOptionService.SaveFromString(poll, options);

        await pollContext.Clients.All.SendAsync("PollCreate", poll);
        return RedirectToAction("Index", "Admin");
    }

    [HttpGet("[action]/{pollId:int}")]
    public IActionResult Edit(int pollId)
    {
        Poll? poll = pollService.GetById(pollId);
        if (poll == null)
        {
            TempData["PollDeleteError"] = $"There was no Poll with id {pollId}";
            return RedirectToAction("Index", "Admin");
        }

        ViewData["Title"] = "Edit poll - Admin Page";
        return View("CreateEdit", poll);
    }

    [HttpPost("[action]/{pollId:int}")]
    public async Task<IActionResult> Edit(int pollId, Poll updatedPoll)
    {
        Poll? poll = pollService.GetById(pollId);

        if (poll == null)
            return RedirectToAction("Index", "Admin");

        poll.Question = updatedPoll.Question;

        string? options = Request.Form["Options"];
        pollOptionService.DeleteAllFromPoll(poll);
        pollOptionService.SaveFromString(poll, options);

        poll.HasVoted = false;
        await pollContext.Clients.All.SendAsync("PollUpdate", poll);
        return RedirectToAction("Index", "Admin");
    }

    [HttpGet("[action]/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        Poll? poll = pollService.GetById(id);

        if (poll == null)
        {
            TempData["PollDeleteError"] = "There was no Poll to delete";
            return RedirectToAction("Index");
        }

        TempData["PollDeleteSuccess"] = "Poll deleted successfully!";

        pollService.Delete(poll);
        await pollContext.Clients.All.SendAsync("PollDelete", poll);
        return RedirectToAction("Index");
    }
}
