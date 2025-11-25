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

        // Check for cookies consent
        if (Request.Cookies.ContainsKey("cookies_accepted"))
        {
            // Get previous last-visit time from cookie, if any
            string? lastVisit = Request.Cookies["lastVisit"];

            // Format the last visit
            if (DateTime.TryParse(lastVisit, out var lastVisitTime))
            {
                ViewBag.LastVisit = "Your last visit was on " + lastVisitTime.ToLocalTime().ToString("f");
            }
            else
            {
                ViewBag.LastVisit = "This is your first visit!";
            }

            // Store current visit time in cookie
            string currentVisit = DateTime.UtcNow.ToString("o");
            Response.Cookies.Append("lastVisit", currentVisit, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(60),
                HttpOnly = false  // NOTE: Vulnerable to XSS, but okay for an MVP
            });
        }

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