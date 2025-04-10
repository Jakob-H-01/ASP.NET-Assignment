using Business.Interfaces;
using Domain.Dtos;
using Domain.Extensions;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class AuthController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;

    public IActionResult Index()
    {
        return LocalRedirect("/login");
    }

    [Route("register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        ViewBag.ErrorMessage = null;

        if (!ModelState.IsValid)
            return View(model);

        string[] names = model.FullName.Split(" ");
        //var registerForm = model.MapTo<RegisterForm>();
        var registerForm = new RegisterForm { Email = model.Email, FirstName = names[0], LastName = names[1], Password = model.Password };
        //registerForm.FirstName = names[0];
        //registerForm.LastName = names[1];

        var result = await _authService.RegisterAsync(registerForm);
        if (result.Succeeded)
            return RedirectToAction("Login", "Auth");

        ViewBag.ErrorMessage = result.Error;
        return View(model);
    }

    [Route("login")]
    public IActionResult Login(string returnUrl = "~/projects")
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "~/projects")
    {
        ViewBag.ErrorMessage = null;
        ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        //var loginForm = model.MapTo<LoginForm>();
        var loginForm = new LoginForm { Email = model.Email, Password = model.Password, RememberMe = model.RememberMe };
        var result = await _authService.LoginAsync(loginForm);
        if (result.Succeeded)
            return LocalRedirect(returnUrl);

        ViewBag.ErrorMessage = result.Error;
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return LocalRedirect("~/");
    }
}
