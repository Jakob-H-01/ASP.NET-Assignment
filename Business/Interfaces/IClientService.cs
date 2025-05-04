using Business.Models;
using Domain.Models;

namespace Business.Interfaces;

public interface IClientService
{
    Task<ServiceResult<bool>> CreateClientAsync(string clientName);
    Task<ServiceResult<Client>> GetClientAsync(string id);
    Task<ServiceResult<IEnumerable<Client>>> GetClientsAsync();
}