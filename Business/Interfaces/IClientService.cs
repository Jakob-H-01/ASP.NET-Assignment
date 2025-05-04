using Business.Models;
using Domain.Models;

namespace Business.Interfaces;

public interface IClientService
{
    Task<ServiceResult<bool>> CreateClientAsync(string clientName);
    Task<ServiceResult<Client>> GetClientByNameAsync(string clientName);
    Task<ServiceResult<IEnumerable<Client>>> GetClientsAsync();
}