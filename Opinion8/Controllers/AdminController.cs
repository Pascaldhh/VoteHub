using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Opinion8.Hubs;
using Opinion8.Models;
using Opinion8.Services;

namespace Opinion8.Controllers;

[Route("/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController(IHubContext<PollHub> pollContext, PollService pollService) : Controller
{
    public IActionResult Index()
    {
        return View(pollService.GetAll());
    }

    [HttpGet("[action]")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("[action]")]
    public IActionResult Create(Poll poll)
    {
        pollService.Save(poll);
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
