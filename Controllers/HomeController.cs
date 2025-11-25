using System.Diagnostics;
using System.Security.Claims;                 // ← THIS WAS MISSING
using Microsoft.AspNetCore.Authorization;       // ← for [Authorize]
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

    // Secured endpoint – replace all nodes for the current user only
    [HttpPost("replace-all-people-nodes")]
    [Authorize]
    public async Task<IActionResult> ReplaceAll([FromBody] List<PersonNode> people)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        // Ensure every node belongs to the current user (defense in depth)
        foreach (var p in people)
            p.UserId = userId;

        await DatabasePersistenceUtility.ReplaceAllPeopleNodesAsync(_db, people, userId);

        return Ok("Database replaced");
    }

    // Secured endpoint – get only the current user's nodes
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