using System.Security.Claims;
using Business.Interfaces;
using Data.Entities;
using Domain.Dtos;
using Domain.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class AuthController(IAuthService authService, SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager, IMemberService memberService) : Controller
{
    private readonly IAuthService _authService = authService;
    private readonly IMemberService _memberService = memberService;
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;
    private readonly UserManager<MemberEntity> _userManager = userManager;

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
        var registerForm = new RegisterForm { Email = model.Email, FirstName = names[0], LastName = names[1], Password = model.Password };

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

        var loginForm = model.MapTo<LoginForm>();
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

    [HttpPost]
    public IActionResult ExternalLogin(string provider, string returnUrl = null!)
    {
        if (string.IsNullOrEmpty(provider))
        {
            ModelState.AddModelError("", "Invalid provider");
            return View("Login");
        }

        var redirectUrl = Url.Action("ExternalLoginCallback", "Auth", new { returnUrl })!;
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null!, string remoteError = null!)
    {
        returnUrl ??= Url.Content("~/projects");

        if (!string.IsNullOrEmpty(remoteError))
        {
            ModelState.AddModelError("", $"Error from external provider: {remoteError}");
            return View("Login");
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return RedirectToAction("Login");

        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (signInResult.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            string firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!;
            string lastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!;
            string email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
            string username = $"ext_{info.LoginProvider.ToLower()}_{email}";

            var member = new MemberEntity { UserName = username, Email = email, FirstName = firstName, LastName = lastName };
            var memberResult = await _userManager.CreateAsync(member);
            var roleResult = await _memberService.SetMemberRoleAsync(member.Id, "Standard");

            if (memberResult.Succeeded && roleResult.Succeeded)
            {
                await _userManager.AddLoginAsync(member, info);
                await _signInManager.SignInAsync(member, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in memberResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("Login");
        }
    }
}
