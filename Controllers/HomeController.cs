using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using family_tree_builder.Models;
using family_tree_builder.Utilities;
using family_tree_builder.Data;

namespace family_tree_builder.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;  // Use _logger.LogInformation("...") to print debug messages to the console
    private readonly ApplicationDbContext _db;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    // Just leads to the home page url
    public IActionResult Index()
    {
        return View();
    }

    // Endpoint to replace all family tree data in the database
    // It uses the persistence utility to interact with the database
    [HttpPost("replace-all-people-nodes")]
    public async Task<IActionResult> ReplaceAll([FromBody] List<PersonNode> people)
    {
        await DatabasePersistenceUtility.ReplaceAllPeopleNodesAsync(_db, people);
        return Ok("Database replaced");
    }

    // Endpoint to fetch all family tree data from the database
    // It uses the persistence utility to interact with the database
    [HttpGet("all-people-nodes")]
    public async Task<IActionResult> GetAll()
    {
        var peopleNodes = await DatabasePersistenceUtility.GetAllPeopleNodesAsync(_db);
        return Ok(peopleNodes);  // Automatically serialized into JSON
    }

    // Error handler
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]  // This tells browser not to cache this page
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
