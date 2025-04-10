using Business.Models;
using Domain.Dtos;

namespace Business.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<bool>> LoginAsync(LoginForm form);
    Task<ServiceResult<bool>> LogoutAsync();
    Task<ServiceResult<bool>> RegisterAsync(RegisterForm form);
}