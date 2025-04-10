using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces;

public interface IMemberService
{
    Task<ServiceResult<bool>> CreateMemberAsync(RegisterForm form, string roleName = "Standard");
    Task<ServiceResult<IEnumerable<Member>>> GetMembersAsync();
    Task<ServiceResult<bool>> SetMemberRoleAsync(string memberId, string roleName);
}