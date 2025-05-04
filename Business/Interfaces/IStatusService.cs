using Business.Models;
using Domain.Models;

namespace Business.Interfaces;

public interface IStatusService
{
    Task<ServiceResult<bool>> CreateStatusAsync(string statusName);
    Task<ServiceResult<bool>> ExistsAsync(string statusName);
    Task<ServiceResult<IEnumerable<Status>>> GetAllStatusAsync();
    Task<ServiceResult<Status>> GetStatusByNameAsync(string statusName);
}