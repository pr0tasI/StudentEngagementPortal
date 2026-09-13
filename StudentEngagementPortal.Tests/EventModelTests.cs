using StudentEngagementPortal.Web.Models;
using Xunit;

namespace StudentEngagementPortal.Tests;

public class EventModelTests
{
    [Fact]
    public void RegisteredCount_ReturnsOnlyRegisteredStudents()
    {
        var ev = new Event
        {
            Title = "Test Event",
            Capacity = 10
        };

        ev.Registrations.Add(new Registration
        {
            UserId = "student1",
            Status = RegistrationStatus.Registered
        });

        ev.Registrations.Add(new Registration
        {
            UserId = "student2",
            Status = RegistrationStatus.Cancelled
        });

        Assert.Equal(1, ev.RegisteredCount);
    }

    [Fact]
    public void IsFull_ReturnsTrue_WhenCapacityIsReached()
    {
        var ev = new Event
        {
            Title = "Test Event",
            Capacity = 2
        };

        ev.Registrations.Add(new Registration
        {
            UserId = "student1",
            Status = RegistrationStatus.Registered
        });

        ev.Registrations.Add(new Registration
        {
            UserId = "student2",
            Status = RegistrationStatus.Registered
        });

        Assert.True(ev.IsFull);
    }

    [Fact]
    public void IsFull_ReturnsFalse_WhenSpacesAreAvailable()
    {
        var ev = new Event
        {
            Title = "Test Event",
            Capacity = 5
        };

        ev.Registrations.Add(new Registration
        {
            UserId = "student1",
            Status = RegistrationStatus.Registered
        });

        Assert.False(ev.IsFull);
    }

    [Fact]
    public void SpacesRemaining_ReturnsCorrectNumber()
    {
        var ev = new Event
        {
            Title = "Test Event",
            Capacity = 10
        };

        ev.Registrations.Add(new Registration
        {
            UserId = "student1",
            Status = RegistrationStatus.Registered
        });

        ev.Registrations.Add(new Registration
        {
            UserId = "student2",
            Status = RegistrationStatus.Registered
        });

        Assert.Equal(8, ev.SpacesRemaining);
    }

    [Fact]
    public void SpacesRemaining_NeverReturnsNegative()
    {
        var ev = new Event
        {
            Title = "Test Event",
            Capacity = 2
        };

        ev.Registrations.Add(new Registration
        {
            UserId = "student1",
            Status = RegistrationStatus.Registered
        });

        ev.Registrations.Add(new Registration
        {
            UserId = "student2",
            Status = RegistrationStatus.Registered
        });

        ev.Registrations.Add(new Registration
        {
            UserId = "student3",
            Status = RegistrationStatus.Registered
        });

        Assert.Equal(0, ev.SpacesRemaining);
    }
}