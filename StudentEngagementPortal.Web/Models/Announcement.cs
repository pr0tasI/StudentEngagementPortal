using System.ComponentModel.DataAnnotations;

namespace StudentEngagementPortal.Web.Models;

public class Announcement
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(3000)]
    public string Body { get; set; } = string.Empty;

    public string PostedByUserId { get; set; } = string.Empty;

    public ApplicationUser? PostedBy { get; set; }

    public DateTime PostedAtUtc { get; set; } = DateTime.UtcNow;
}