using Microsoft.AspNetCore.Identity;

namespace StudentEngagementPortal.Web.Models;

/// <summary>
/// Extends IdentityUser rather than creating a separate Admins table.
/// The Role claim (assigned via ASP.NET Core Identity roles: "Student" / "Admin")
/// drives all role-based authorisation, consistent with the design report's
/// decision to avoid a separate Admins table (see Section: Database Design).
/// </summary>
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<Announcement> AnnouncementsPosted { get; set; } = new List<Announcement>();
    public ICollection<Event> EventsCreated { get; set; } = new List<Event>();
}
