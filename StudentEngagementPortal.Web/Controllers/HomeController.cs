using Microsoft.AspNetCore.Mvc;

namespace StudentEngagementPortal.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
