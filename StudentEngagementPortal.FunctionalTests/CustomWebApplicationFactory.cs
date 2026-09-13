using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudentEngagementPortal.Data;

namespace StudentEngagementPortal.FunctionalTests;

// Boots the real app (Program.cs) but swaps the SQL Server DbContext for
// an InMemory one, so functional tests don't need a real database server.
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase($"FunctionalTestsDb-{Guid.NewGuid()}"));
        });
    }
}
