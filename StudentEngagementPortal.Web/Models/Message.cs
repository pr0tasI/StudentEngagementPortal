using System.ComponentModel.DataAnnotations;

namespace StudentEngagementPortal.Web.Models;

public enum MessageStatus
{
    Open,
    InProgress,
    Resolved
}

/// <summary>
/// Student support request ("Messages" table in the design report).
/// Status progresses Open -> InProgress -> Resolved as Admins triage it.
/// </summary>
public class Message
{
    public int Id { get; set; }

    [Required]
    public string StudentUserId { get; set; } = string.Empty;
    public ApplicationUser? Student { get; set; }

    [Required, StringLength(150)]
    public string Subject { get; set; } = string.Empty;

    [Required, StringLength(2000)]
    public string Body { get; set; } = string.Empty;

    public MessageStatus Status { get; set; } = MessageStatus.Open;

    [StringLength(2000)]
    public string? AdminResponse { get; set; }

    public DateTime SubmittedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? RespondedAtUtc { get; set; }
}
