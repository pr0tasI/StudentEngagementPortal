using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEngagementPortal.Web.Data;
using StudentEngagementPortal.Web.Models;

namespace StudentEngagementPortal.Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(
        ApplicationDbContext db,
        UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    // Admin dashboard
    public async Task<IActionResult> Index()
    {
        var events = await _db.Events
            .Include(e => e.Registrations)
            .OrderBy(e => e.EventDateTime)
            .ToListAsync();

        return View(events);
    }

    // Create event - page
    [HttpGet]
    public IActionResult CreateEvent()
    {
        return View();
    }

    // Create event - submit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEvent(Event model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return Forbid();

        model.CreatedByUserId = userId;

        _db.Events.Add(model);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Event created successfully.";

        return RedirectToAction(nameof(Index));
    }

    // Edit event - page
    [HttpGet]
    public async Task<IActionResult> EditEvent(int id)
    {
        var ev = await _db.Events.FindAsync(id);

        if (ev == null)
            return NotFound();

        return View(ev);
    }

    // Edit event - submit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEvent(int id, Event model)
    {
        if (id != model.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(model);

        var ev = await _db.Events.FindAsync(id);

        if (ev == null)
            return NotFound();

        ev.Title = model.Title;
        ev.Description = model.Description;
        ev.Location = model.Location;
        ev.Category = model.Category;
        ev.EventDateTime = model.EventDateTime;
        ev.Capacity = model.Capacity;

        await _db.SaveChangesAsync();

        TempData["Success"] = "Event updated successfully.";

        return RedirectToAction(nameof(Index));
    }

    // Cancel event
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelEvent(int id)
    {
        var ev = await _db.Events.FindAsync(id);

        if (ev == null)
            return NotFound();

        ev.IsCancelled = true;

        await _db.SaveChangesAsync();

        TempData["Success"] = "Event cancelled successfully.";

        return RedirectToAction(nameof(Index));
    }

    // Delete event - confirmation page
    [HttpGet]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var ev = await _db.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (ev == null)
            return NotFound();

        return View(ev);
    }

    // Delete event - submit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteEventConfirmed(int id)
    {
        var ev = await _db.Events
            .FirstOrDefaultAsync(e => e.Id == id);

        if (ev == null)
            return NotFound();

        _db.Events.Remove(ev);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Event deleted successfully.";

        return RedirectToAction(nameof(Index));
    }

    // View student sign-ups for an event
    [HttpGet]
    public async Task<IActionResult> SignUps(int id)
    {
        var ev = await _db.Events
            .Include(e => e.Registrations)
                .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (ev == null)
            return NotFound();

        var registrations = ev.Registrations
            .Where(r => r.Status == RegistrationStatus.Registered)
            .OrderBy(r => r.RegisteredAtUtc)
            .ToList();

        ViewBag.Event = ev;

        return View(registrations);
    }

    // Announcements - list
    [HttpGet]
    public async Task<IActionResult> Announcements()
    {
        var announcements = await _db.Announcements
            .Include(a => a.PostedBy)
            .OrderByDescending(a => a.PostedAtUtc)
            .ToListAsync();

        return View(announcements);
    }

    // Create announcement - page
    [HttpGet]
    public IActionResult CreateAnnouncement()
    {
        return View();
    }

    // Create announcement - submit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAnnouncement(Announcement model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return Forbid();

        model.PostedByUserId = userId;
        model.PostedAtUtc = DateTime.UtcNow;

        _db.Announcements.Add(model);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Announcement posted successfully.";

        return RedirectToAction(nameof(Announcements));
    }
    // Delete announcement
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteAnnouncement(int id)
{
    var announcement = await _db.Announcements.FindAsync(id);

    if (announcement == null)
        return NotFound();

    _db.Announcements.Remove(announcement);
    await _db.SaveChangesAsync();

    TempData["Success"] = "Announcement deleted successfully.";

    return RedirectToAction(nameof(Announcements));
}
}
