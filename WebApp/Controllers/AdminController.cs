﻿using Business.Interfaces;
using Domain.Dtos;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin")]
public class AdminController(IMemberService memberService, IAuthService authService) : Controller
{
    private readonly IMemberService _memberService = memberService;
    private readonly IAuthService _authService = authService;

    [AllowAnonymous]
    [Route("login")]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "~/admin/members")
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

        var memberForm = model.MapTo<MemberCreationForm>();

        if (model.Day != null && model.Month != null && model.Year != null)
        {
            var dateOfBirth = new DateTime((int)model.Year, (int)model.Month, (int)model.Day);
            memberForm.DateOfBirth = dateOfBirth;
        }

        await _memberService.CreateMemberAsync(memberForm, model.RoleName);

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

        var memberForm = model.MapTo<MemberUpdateForm>();

        if (model.Day != null && model.Month != null && model.Year != null)
        {
            var dateOfBirth = new DateTime((int)model.Year, (int)model.Month, (int)model.Day);
            memberForm.DateOfBirth = dateOfBirth;
        }

        await _memberService.UpdateMemberAsync(memberForm, model.RoleName);

        return RedirectToAction("TeamMembers");
    }

    [HttpPost]
    [Route("members/delete")]
    public async Task<IActionResult> DeleteMember(string id)
    {
        await _memberService.DeleteMemberAsync(id);

        return RedirectToAction("TeamMembers");
    }
}
