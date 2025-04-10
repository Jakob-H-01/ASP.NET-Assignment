using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Authorize]
[Route("projects")]
public class ProjectsController : Controller
{
    public IActionResult Projects()
    {
        return View();
    }
}
