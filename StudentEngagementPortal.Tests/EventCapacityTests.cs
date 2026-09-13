using StudentEngagementPortal.Models;
using Xunit;

namespace StudentEngagementPortal.Tests;

// Covers the capacity logic in Event (used by EventsController.Register to
// decide whether a student can sign up) — the core business rule the
// brief asks to be verified with unit tests.
public class EventCapacityTests
{
    private static Event MakeEvent(int capacity) => new()
    {
        Id = 1,
        Title = "Test Event",
        Description = "desc",
        Category = "Social",
        Location = "Hall",
        StartsAt = DateTime.UtcNow.AddDays(1),
        Capacity = capacity,
        Registrations = new List<Registration>()
    };

    [Fact]
    public void ActiveRegistrationCount_CountsOnlyConfirmedRegistrations()
    {
        var ev = MakeEvent(capacity: 10);
        ev.Registrations.Add(new Registration { UserId = "u1", Status = RegistrationStatus.Confirmed });
        ev.Registrations.Add(new Registration { UserId = "u2", Status = RegistrationStatus.Confirmed });
        ev.Registrations.Add(new Registration { UserId = "u3", Status = RegistrationStatus.Cancelled });

        Assert.Equal(2, ev.ActiveRegistrationCount);
    }

    [Fact]
    public void IsFull_WhenConfirmedCountBelowCapacity_ReturnsFalse()
    {
        var ev = MakeEvent(capacity: 2);
        ev.Registrations.Add(new Registration { UserId = "u1", Status = RegistrationStatus.Confirmed });

        Assert.False(ev.IsFull);
    }

    [Fact]
    public void IsFull_WhenConfirmedCountReachesCapacity_ReturnsTrue()
    {
        var ev = MakeEvent(capacity: 2);
        ev.Registrations.Add(new Registration { UserId = "u1", Status = RegistrationStatus.Confirmed });
        ev.Registrations.Add(new Registration { UserId = "u2", Status = RegistrationStatus.Confirmed });

        Assert.True(ev.IsFull);
    }

    [Fact]
    public void IsFull_IgnoresCancelledRegistrations_SoASpotFreesUp()
    {
        var ev = MakeEvent(capacity: 1);
        ev.Registrations.Add(new Registration { UserId = "u1", Status = RegistrationStatus.Cancelled });

        Assert.False(ev.IsFull);
    }

    [Fact]
    public void Registration_DefaultsToConfirmedStatus()
    {
        var registration = new Registration { UserId = "u1", EventId = 1 };

        Assert.Equal(RegistrationStatus.Confirmed, registration.Status);
    }
}
