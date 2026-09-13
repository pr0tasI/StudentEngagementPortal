using System.Net;
using Xunit;

namespace StudentEngagementPortal.FunctionalTests;

// Exercises real HTTP requests through the app pipeline (routing,
// Identity's cookie auth, [Authorize] attributes) rather than calling
// controller actions directly — this is what the brief calls
// "functional testing to verify page flows".
public class PageFlowTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PageFlowTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task HomePage_LoadsSuccessfully()
    {
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task LoginPage_LoadsSuccessfully()
    {
        var response = await _client.GetAsync("/Account/Login");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task RegisterPage_LoadsSuccessfully()
    {
        var response = await _client.GetAsync("/Account/Register");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task EventsIndex_WhenNotAuthenticated_RedirectsToLogin()
    {
        // Events is behind [Authorize], so an anonymous request should be
        // redirected to the login page rather than served directly.
        var response = await _client.GetAsync("/Events");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Account/Login", response.Headers.Location?.ToString());
    }

    [Fact]
    public async Task AdminDashboard_WhenNotAuthenticated_RedirectsToLogin()
    {
        var response = await _client.GetAsync("/Admin/Dashboard");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Account/Login", response.Headers.Location?.ToString());
    }

    [Fact]
    public async Task AnnouncementsIndex_WhenNotAuthenticated_RedirectsToLogin()
    {
        var response = await _client.GetAsync("/Announcements");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
    }
}
