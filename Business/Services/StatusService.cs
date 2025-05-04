using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Extensions;
using Domain.Models;

namespace Business.Services;

public class StatusService(IStatusRepository statusRepository) : IStatusService
{
    private readonly IStatusRepository _statusRepository = statusRepository;

    public async Task<ServiceResult<bool>> CreateStatusAsync(string statusName)
    {
        if (statusName == null)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 400, Error = "Status name can't be null" };

        var exists = await _statusRepository.ExistsAsync(x => x.StatusName == statusName);
        if (exists.Succeeded)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 409, Error = "A status with the same name already exists" };

        var entity = new StatusEntity { StatusName = statusName };
        var result = await _statusRepository.AddAsync(entity);
        return result.MapTo<ServiceResult<bool>>();
    }

    public async Task<ServiceResult<IEnumerable<Status>>> GetAllStatusAsync()
    {
        var result = await _statusRepository.GetAllAsync();
        return result.MapTo<ServiceResult<IEnumerable<Status>>>();
    }

    public async Task<ServiceResult<Status>> GetStatusByNameAsync(string statusName)
    {
        var result = await _statusRepository.GetAsync(x => x.StatusName == statusName);
        return result.MapTo<ServiceResult<Status>>();
    }

    public async Task<ServiceResult<bool>> ExistsAsync(string statusName)
    {
        var result = await _statusRepository.ExistsAsync(x => x.StatusName == statusName);
        return result.MapTo<ServiceResult<bool>>();
    }
}