using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces;

public interface IMemberService
{
    Task<ServiceResult<bool>> CreateMemberAsync(MemberCreationForm form, string roleName = "Standard");
    Task<ServiceResult<bool>> DeleteMemberAsync(string id);
    Task<ServiceResult<Member>> GetMemberAsync(string id);
    Task<ServiceResult<IEnumerable<Member>>> GetMembersAsync();
    Task<ServiceResult<bool>> SetMemberRoleAsync(string memberId, string roleName);
}