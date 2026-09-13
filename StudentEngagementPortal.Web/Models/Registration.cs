namespace StudentEngagementPortal.Web.Models;

public enum RegistrationStatus
{
    Registered,
    Cancelled
}

/// <summary>
/// Join table associating a Student (ApplicationUser) with an Event.
/// Carries status and timestamp as specified in the design report's
/// Summary of Core Database Tables.
/// </summary>
public class Registration
{
    public int Id { get; set; }

    [System.ComponentModel.DataAnnotations.Required]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    public int EventId { get; set; }
    public Event? Event { get; set; }

    public RegistrationStatus Status { get; set; } = RegistrationStatus.Registered;

    public DateTime RegisteredAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? CancelledAtUtc { get; set; }
}
