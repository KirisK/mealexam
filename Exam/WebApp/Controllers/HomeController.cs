using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.v1;
using WebApp.ViewModels;

#pragma warning disable 1591

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (User.IsInRole("User"))
        {
            return RedirectToAction("Index", nameof(Recipe));
        }
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}