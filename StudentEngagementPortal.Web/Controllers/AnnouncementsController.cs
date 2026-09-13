using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEngagementPortal.Web.Data;

namespace StudentEngagementPortal.Web.Controllers;

[Authorize]
public class AnnouncementsController : Controller
{
    private readonly ApplicationDbContext _db;

    public AnnouncementsController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var announcements = await _db.Announcements
            .Include(a => a.PostedBy)
            .OrderByDescending(a => a.PostedAtUtc)
            .ToListAsync();

        return View(announcements);
    }
}