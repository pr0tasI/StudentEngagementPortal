using Microsoft.AspNetCore.Identity;
using StudentEngagementPortal.Web.Models;

namespace StudentEngagementPortal.Web.Data;

/// <summary>
/// Seeds the two application roles (Student, Admin), a demo account for each,
/// and a handful of sample events so the portal is demonstrable immediately
/// after `dotnet ef database update`. Called once from Program.cs at startup.
/// </summary>
public static class DbSeeder
{
    public const string AdminRole = "Admin";
    public const string StudentRole = "Student";

    public static async Task SeedAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var context = services.GetRequiredService<ApplicationDbContext>();

        foreach (var role in new[] { AdminRole, StudentRole })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

       var admin = await userManager.FindByEmailAsync("admin@portal.local");

if (admin is null)
{
    admin = new ApplicationUser
    {
        UserName = "admin@portal.local",
        Email = "admin@portal.local",
        FullName = "Yehor Protasevych",
        EmailConfirmed = true
    };

    var result = await userManager.CreateAsync(admin, "Admin#Pass123");

    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(admin, AdminRole);
    }
}
else
{
    admin.FullName = "Yehor Protasevych";
    await userManager.UpdateAsync(admin);
}

        if (!context.Events.Any())
        {
            context.Events.AddRange(
                new Event
                {
                    Title = "Freshers' Welcome Fair",
                    Description = "Meet campus societies and support services in one afternoon.",
                    Location = "Main Hall",
                    Category = "Social",
                    EventDateTime = DateTime.UtcNow.AddDays(7),
                    Capacity = 200,
                    CreatedByUserId = admin.Id
                },
                new Event
                {
                    Title = "Careers in Tech Panel",
                    Description = "Panel discussion with graduates working in software engineering.",
                    Location = "Lecture Theatre B",
                    Category = "Careers",
                    EventDateTime = DateTime.UtcNow.AddDays(14),
                    Capacity = 80,
                    CreatedByUserId = admin.Id
                },
                new Event
                {
                    Title = "Wellbeing Workshop",
                    Description = "Drop-in session with the student wellbeing team.",
                    Location = "Room 214",
                    Category = "Wellbeing",
                    EventDateTime = DateTime.UtcNow.AddDays(3),
                    Capacity = 25,
                    CreatedByUserId = admin.Id
                }
            );

            context.Announcements.Add(new Announcement
            {
                Title = "Portal launch",
                Body = "Welcome to the new Student Engagement Portal — browse events, register, and reach support here.",
                PostedByUserId = admin.Id
            });

            await context.SaveChangesAsync();
        }
    }
}
