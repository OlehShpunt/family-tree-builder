using System.Diagnostics;
using System.Security.Claims;                 
using Microsoft.AspNetCore.Authorization;      
using Microsoft.AspNetCore.Mvc;
using family_tree_builder.Models;
using family_tree_builder.Utilities;
using family_tree_builder.Data;

namespace family_tree_builder.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }

 
    [HttpPost("replace-all-people-nodes")]
    [Authorize]
    public async Task<IActionResult> ReplaceAll([FromBody] List<PersonNode> people)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

    
        foreach (var p in people)
            p.UserId = userId;

        await DatabasePersistenceUtility.ReplaceAllPeopleNodesAsync(_db, people, userId);

        return Ok("Database replaced");
    }


    [HttpGet("all-people-nodes")]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var peopleNodes = await DatabasePersistenceUtility.GetAllPeopleNodesAsync(_db, userId);

        return Ok(peopleNodes);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}