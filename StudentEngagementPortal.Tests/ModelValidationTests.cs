using System.ComponentModel.DataAnnotations;
using StudentEngagementPortal.Models.ViewModels;
using Xunit;

namespace StudentEngagementPortal.Tests;

// Validates the DataAnnotation rules on the form view models used by
// EventsController and MessagesController — these enforce the "at least
// 1" capacity rule and required fields before a request ever reaches EF Core.
public class ModelValidationTests
{
    private static List<ValidationResult> Validate(object model)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        return results;
    }

    [Fact]
    public void EventFormViewModel_WithZeroCapacity_FailsValidation()
    {
        var model = new EventFormViewModel
        {
            Title = "Test",
            Description = "Desc",
            Category = "Social",
            Location = "Hall",
            StartsAt = DateTime.Now.AddDays(1),
            Capacity = 0
        };

        var results = Validate(model);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(EventFormViewModel.Capacity)));
    }

    [Fact]
    public void EventFormViewModel_WithValidData_PassesValidation()
    {
        var model = new EventFormViewModel
        {
            Title = "Test",
            Description = "Desc",
            Category = "Social",
            Location = "Hall",
            StartsAt = DateTime.Now.AddDays(1),
            Capacity = 30
        };

        var results = Validate(model);

        Assert.Empty(results);
    }

    [Fact]
    public void RegisterViewModel_WithMismatchedPasswords_FailsValidation()
    {
        var model = new RegisterViewModel
        {
            FullName = "Amara Osei",
            Email = "amara@example.ac.uk",
            Password = "Password1",
            ConfirmPassword = "Password2"
        };

        var results = Validate(model);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(RegisterViewModel.ConfirmPassword)));
    }

    [Fact]
    public void ContactFormViewModel_WithoutSubject_FailsValidation()
    {
        var model = new ContactFormViewModel
        {
            Subject = "",
            Body = "I need help with my registration."
        };

        var results = Validate(model);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(ContactFormViewModel.Subject)));
    }
}
