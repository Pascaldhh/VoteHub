using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Opinion8.Data;
using Opinion8.Models;
using Opinion8.Services;

namespace Opinion8.Controllers;

public class HomeController(PollService pollService) : Controller
{
    public IActionResult Index()
    {
        return View(pollService.GetAll());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
