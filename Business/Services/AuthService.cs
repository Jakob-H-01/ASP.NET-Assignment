using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Domain.Dtos;
using Domain.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class AuthService(IMemberService memberService, SignInManager<MemberEntity> signInManager) : IAuthService
{
    private readonly IMemberService _memberService = memberService;
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;

    public async Task<ServiceResult<bool>> LoginAsync(LoginForm form)
    {
        if (form == null)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 400, Error = "Form can't be null" };

        var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, form.RememberMe, false);
        return result.Succeeded
            ? new ServiceResult<bool> { Succeeded = true, StatusCode = 200 }
            : new ServiceResult<bool> { Succeeded = false, StatusCode = 401, Error = "Invalid email or password" };
    }

    public async Task<ServiceResult<bool>> RegisterAsync(RegisterForm form)
    {
        if (form == null)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 400, Error = "Form can't be null" };

        //var memberForm = form.MapTo<MemberCreationForm>();
        var result = await _memberService.CreateMemberAsync(form);
        return result.Succeeded
            ? new ServiceResult<bool> { Succeeded = true, StatusCode = 201 }
            : new ServiceResult<bool> { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<ServiceResult<bool>> LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return new ServiceResult<bool> { Succeeded = true, StatusCode = 200 };
    }
}