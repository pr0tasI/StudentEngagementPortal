using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEngagementPortal.Web.Data;
using StudentEngagementPortal.Web.Models;

namespace StudentEngagementPortal.Web.Controllers;

[Authorize]
public class EventsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public EventsController(
        ApplicationDbContext db,
        UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    // GET /Events
    public async Task<IActionResult> Index(string? category)
    {
        var query = _db.Events
            .Include(e => e.Registrations)
            .Where(e => !e.IsCancelled && e.EventDateTime >= DateTime.UtcNow)
            .OrderBy(e => e.EventDateTime)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(e => e.Category == category);
        }

        ViewBag.Categories = await _db.Events
            .Select(e => e.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        ViewBag.SelectedCategory = category;

        return View(await query.ToListAsync());
    }

    // GET /Events/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var ev = await _db.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (ev == null)
            return NotFound();

        var userId = _userManager.GetUserId(User);

        ViewBag.IsRegistered = ev.Registrations.Any(
            r => r.UserId == userId &&
                 r.Status == RegistrationStatus.Registered);

        return View(ev);
    }

    // POST /Events/Register/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(int id)
    {
        var ev = await _db.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (ev == null)
            return NotFound();

        var userId = _userManager.GetUserId(User)!;

        var existing = ev.Registrations
            .FirstOrDefault(r => r.UserId == userId);

        if (existing != null &&
            existing.Status == RegistrationStatus.Registered)
        {
            TempData["Info"] = "You are already registered for this event.";
            return RedirectToAction(nameof(Details), new { id });
        }

        if (ev.IsFull)
        {
            TempData["Error"] = "Sorry — this event is now full.";
            return RedirectToAction(nameof(Details), new { id });
        }

        if (existing != null)
        {
            existing.Status = RegistrationStatus.Registered;
            existing.RegisteredAtUtc = DateTime.UtcNow;
            existing.CancelledAtUtc = null;
        }
        else
        {
            _db.Registrations.Add(new Registration
            {
                EventId = ev.Id,
                UserId = userId,
                Status = RegistrationStatus.Registered,
                RegisteredAtUtc = DateTime.UtcNow
            });
        }

        await _db.SaveChangesAsync();

        TempData["Success"] = "You're registered! See you there.";

        return RedirectToAction(nameof(Details), new { id });
    }

    // POST /Events/Unregister/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Unregister(int id)
    {
        var userId = _userManager.GetUserId(User);

        var registration = await _db.Registrations
            .FirstOrDefaultAsync(r =>
                r.EventId == id &&
                r.UserId == userId &&
                r.Status == RegistrationStatus.Registered);

        if (registration != null)
        {
            registration.Status = RegistrationStatus.Cancelled;
            registration.CancelledAtUtc = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            TempData["Success"] = "Registration cancelled.";
        }

        return RedirectToAction(nameof(Details), new { id });
    }
}
