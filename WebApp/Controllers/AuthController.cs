using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class AuthController : Controller
{
    public IActionResult Index()
    {
        return LocalRedirect("/login");
    }

    [Route("register")]
    public IActionResult Register()
    {
        return View();
    }

    [Route("login")]
    public IActionResult Login()
    {
        return View();
    }
}
