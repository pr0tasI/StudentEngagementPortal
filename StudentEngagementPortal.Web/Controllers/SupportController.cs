using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEngagementPortal.Web.Data;
using StudentEngagementPortal.Web.Models;

namespace StudentEngagementPortal.Web.Controllers;

[Authorize]
public class SupportController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public SupportController(
        ApplicationDbContext db,
        UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new Message());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(Message model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = _userManager.GetUserId(User);

        if (userId == null)
            return Forbid();

        var message = new Message
        {
            StudentUserId = userId,
            Subject = model.Subject,
            Body = model.Body,
            Status = MessageStatus.Open,
            SubmittedAtUtc = DateTime.UtcNow
        };

        _db.Messages.Add(message);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Your support request has been submitted.";

        return RedirectToAction(nameof(Index));
    }
}