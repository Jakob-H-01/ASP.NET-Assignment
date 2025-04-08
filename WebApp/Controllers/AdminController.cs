using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

[Route("admin")]
public class AdminController : Controller
{
    [Route("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [Route("login")]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        return Redirect("/admin/members");
    }
    
    [Route("members")]
    public IActionResult TeamMembers()
    {
        return View();
    }
}
