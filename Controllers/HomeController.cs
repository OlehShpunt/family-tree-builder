using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using family_tree_builder.Models;

namespace family_tree_builder.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;  // Use _logger.LogInformation("...") to print debug messages to the console

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]  // This tells browser not to cache this page
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
