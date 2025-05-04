using System.Diagnostics;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class MemberService(IMemberRepository memberRepository, UserManager<MemberEntity> userManager, RoleManager<IdentityRole> roleManager) : IMemberService
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task<ServiceResult<bool>> CreateMemberAsync(MemberCreationForm form, string roleName = "Standard")
    {
        if (form == null)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 400, Error = "Form can't be null" };

        var exists = await _memberRepository.ExistsAsync(x => x.Email == form.Email);
        if (exists.Succeeded)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 409, Error = "A member with the same email already exists" };

        try
        {
            var memberEntity = form.MapTo<MemberEntity>();
            memberEntity.UserName = form.Email;

            var result = await _userManager.CreateAsync(memberEntity, form.Password ?? "Temp123!");
            if (result.Succeeded)
            {
                var setRoleResult = await SetMemberRoleAsync(memberEntity.Id, roleName);
                return setRoleResult.Succeeded
                    ? new ServiceResult<bool> { Succeeded = true, StatusCode = 201 }
                    : new ServiceResult<bool> { Succeeded = false, StatusCode = 201, Error = "Member created but not added to role" };
            }

            return new ServiceResult<bool> { Succeeded = false, StatusCode = 500, Error = "Unable to create member" };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public async Task<ServiceResult<IEnumerable<Member>>> GetMembersAsync()
    {
        var result = await _memberRepository.GetAllAsync();
        return result.MapTo<ServiceResult<IEnumerable<Member>>>();
    }

    public async Task<ServiceResult<Member>> GetMemberAsync(string id)
    {
        var result = await _memberRepository.GetAsync(x => x.Id == id);
        return result.MapTo<ServiceResult<Member>>();
    }

    public async Task<ServiceResult<bool>> UpdateMemberAsync(MemberUpdateForm form, string roleName)
    {
        if (form == null)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 400, Error = "Form can't be null" };

        try
        {
            var memberResult = await _memberRepository.GetEntityAsync(x => x.Id == form.Id);
            if (!memberResult.Succeeded)
                return new ServiceResult<bool> { Succeeded = false, StatusCode = 404, Error = "Member doesn't exist" };

            var memberEntity = memberResult.Result;
            memberEntity.MapFrom(form);

            var result = await _memberRepository.UpdateAsync(memberEntity!);
            if (result.Succeeded)
            {
                var setRoleResult = await SetMemberRoleAsync(memberEntity!.Id, roleName);
                return setRoleResult.Succeeded
                    ? new ServiceResult<bool> { Succeeded = true, StatusCode = 200 }
                    : new ServiceResult<bool> { Succeeded = false, StatusCode = 200, Error = "Member updated but not added to role" };
            }

            return new ServiceResult<bool> { Succeeded = false, StatusCode = 500, Error = "Unable to update member" };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public async Task<ServiceResult<bool>> DeleteMemberAsync(string id)
    {
        var result = await _memberRepository.DeleteAsync(x => x.Id == id);
        return result.MapTo<ServiceResult<bool>>();
    }

    public async Task<ServiceResult<bool>> SetMemberRoleAsync(string memberId, string roleName)
    {
        if (memberId == null || roleName == null)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 400, Error = "Both memberId and roleName must be provided" };

        if (!await _roleManager.RoleExistsAsync(roleName))
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 404, Error = "Role doesn't exist" };

        var member = await _userManager.FindByIdAsync(memberId);
        if (member == null)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 404, Error = "Member doesn't exist" };

        var memberRoles = await _userManager.GetRolesAsync(member);
        if (memberRoles.Count > 0)
        {
            foreach (var role in memberRoles)
            {
                if (role != roleName)
                    await _userManager.RemoveFromRoleAsync(member, role);
            }
        }

        memberRoles = await _userManager.GetRolesAsync(member);
        if (memberRoles.Count > 0)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 500, Error = "Unable to remove previous member role(s)" };

        if (await _userManager.IsInRoleAsync(member, roleName))
            return new ServiceResult<bool> { Succeeded = true, StatusCode = 200 };

        var result = await _userManager.AddToRoleAsync(member, roleName);
        return result.Succeeded
            ? new ServiceResult<bool> { Succeeded = true, StatusCode = 200 }
            : new ServiceResult<bool> { Succeeded = false, StatusCode = 500, Error = "Unable to set member role" };
    }
}