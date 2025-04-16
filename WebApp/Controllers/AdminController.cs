using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin")]
public class AdminController(IMemberService memberService) : Controller
{
    private readonly IMemberService _memberService = memberService;

    [AllowAnonymous]
    [Route("login")]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        return Redirect("/admin/members");
    }

    [Route("members")]
    public async Task<IActionResult> TeamMembers()
    {
        var result = await _memberService.GetMembersAsync();
        var members = result.Result;

        var model = new TeamMembersViewModel
        {
            Members = members!
        };

        return View(model);
    }
    
    [HttpPost]
    [Route("members/add")]
    public async Task<IActionResult> AddMember([Bind(Prefix = "AddMember")] AddMemberViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var result = await _memberService.GetMembersAsync();
            var members = result.Result;
            var viewModel = new TeamMembersViewModel
            {
                Members = members!,
                AddMember = model,
                ShowAddModal = true
            };

            return View("TeamMembers", viewModel);
        }

        return RedirectToAction("TeamMembers");
    }

    [HttpPost]
    [Route("members/edit")]
    public async Task<IActionResult> EditMember([Bind(Prefix = "EditMember")] EditMemberViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var result = await _memberService.GetMembersAsync();
            var members = result.Result;
            var viewModel = new TeamMembersViewModel
            {
                Members = members!,
                EditMember = model,
                ShowEditModal = true
            };

            return View("TeamMembers", viewModel);
        }

        return RedirectToAction("TeamMembers");
    }
}
