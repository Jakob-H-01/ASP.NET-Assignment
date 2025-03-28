using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Route("admin")]
public class AdminController : Controller
{
    [Route("login")]
    public IActionResult Login()
    {
        return View();
    }
    
    [Route("members")]
    public IActionResult TeamMembers()
    {
        return View();
    }
}
