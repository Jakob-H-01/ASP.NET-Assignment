using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Extensions;
using Domain.Models;

namespace Business.Services;

public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<ServiceResult<bool>> CreateClientAsync(string clientName)
    {
        if (clientName == null)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 400, Error = "Client name can't be null" };

        var exists = await _clientRepository.ExistsAsync(x => x.ClientName == clientName);
        if (exists.Succeeded)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 409, Error = "A client with the same name already exists" };

        var entity = new ClientEntity { ClientName = clientName };
        var result = await _clientRepository.AddAsync(entity);
        return result.MapTo<ServiceResult<bool>>();
    }

    public async Task<ServiceResult<IEnumerable<Client>>> GetClientsAsync()
    {
        var result = await _clientRepository.GetAllAsync();
        return result.MapTo<ServiceResult<IEnumerable<Client>>>();
    }

    public async Task<ServiceResult<Client>> GetClientAsync(string id)
    {
        var result = await _clientRepository.GetAsync(x => x.Id == id);
        return result.MapTo<ServiceResult<Client>>();
    }

    public async Task<ServiceResult<bool>> ExistsAsync(string clientName)
    {
        var result = await _clientRepository.ExistsAsync(x => x.ClientName == clientName);
        return result.MapTo<ServiceResult<bool>>();
    }
}