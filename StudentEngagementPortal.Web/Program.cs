using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentEngagementPortal.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// EF Core — SQLite by default for easy local/demo use; connection string in
// appsettings.json can be swapped to SQL Server without touching this file.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=studentengagementportal.db";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// ASP.NET Core Identity with role support (Student / Admin).
builder.Services.AddIdentity<StudentEngagementPortal.Web.Models.ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Apply pending migrations and seed roles/demo data on startup — keeps the
// "clone, dotnet ef database update, dotnet run" path simple for the demo.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
    await DbSeeder.SeedAsync(scope.ServiceProvider);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Exposed for WebApplicationFactory<Program> in the functional test project.
public partial class Program { }
