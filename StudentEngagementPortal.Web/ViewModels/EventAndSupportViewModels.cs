using System.ComponentModel.DataAnnotations;

namespace StudentEngagementPortal.Web.ViewModels;

public class EventFormViewModel
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
    [DataType(DataType.DateTime)]
    public DateTime EventDateTime { get; set; } = DateTime.Now.AddDays(1);

    [Range(1, 10000)]
    public int Capacity { get; set; } = 20;
}

public class ContactFormViewModel
{
    [Required, StringLength(150)]
    public string Subject { get; set; } = string.Empty;

    [Required, StringLength(2000)]
    [DataType(DataType.MultilineText)]
    public string Body { get; set; } = string.Empty;
}

public class MessageResponseViewModel
{
    public int MessageId { get; set; }

    [Required, StringLength(2000)]
    [Display(Name = "Response")]
    public string AdminResponse { get; set; } = string.Empty;
}

public class AnnouncementFormViewModel
{
    [Required, StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(3000)]
    [DataType(DataType.MultilineText)]
    public string Body { get; set; } = string.Empty;
}
