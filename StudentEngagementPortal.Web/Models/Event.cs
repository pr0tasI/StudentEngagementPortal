using System.ComponentModel.DataAnnotations;

namespace StudentEngagementPortal.Web.Models;

public class Event
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required, StringLength(150)]
    public string Location { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Category { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Date and Time")]
    public DateTime EventDateTime { get; set; }

    [Range(1, 10000)]
    public int Capacity { get; set; }

    public bool IsCancelled { get; set; }

    public string CreatedByUserId { get; set; } = string.Empty;
    public ApplicationUser? CreatedBy { get; set; }

    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    // Not mapped — computed convenience properties used by views
    public int RegisteredCount => Registrations.Count(r => r.Status == RegistrationStatus.Registered);
    public bool IsFull => RegisteredCount >= Capacity;
    public int SpacesRemaining => Math.Max(0, Capacity - RegisteredCount);
}
