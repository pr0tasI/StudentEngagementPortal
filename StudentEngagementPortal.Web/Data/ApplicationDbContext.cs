using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentEngagementPortal.Web.Models;

namespace StudentEngagementPortal.Web.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Event> Events => Set<Event>();
    public DbSet<Registration> Registrations => Set<Registration>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Announcement> Announcements => Set<Announcement>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Event -> CreatedBy (Admin). Restrict delete so an Admin account
        // cannot be removed while events still reference it.
        builder.Entity<Event>()
            .HasOne(e => e.CreatedBy)
            .WithMany(u => u.EventsCreated)
            .HasForeignKey(e => e.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Registration -> Event (cascade: deleting an event clears its registrations)
        builder.Entity<Registration>()
            .HasOne(r => r.Event)
            .WithMany(e => e.Registrations)
            .HasForeignKey(r => r.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // Registration -> User (restrict: a user's history isn't silently deleted)
        builder.Entity<Registration>()
            .HasOne(r => r.User)
            .WithMany(u => u.Registrations)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // A student may only hold one active registration per event.
        builder.Entity<Registration>()
            .HasIndex(r => new { r.UserId, r.EventId })
            .IsUnique();

        builder.Entity<Message>()
            .HasOne(m => m.Student)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.StudentUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Announcement>()
            .HasOne(a => a.PostedBy)
            .WithMany(u => u.AnnouncementsPosted)
            .HasForeignKey(a => a.PostedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Event>()
            .HasIndex(e => e.EventDateTime);

        builder.Entity<Announcement>()
            .HasIndex(a => a.PostedAtUtc);
    }
}
