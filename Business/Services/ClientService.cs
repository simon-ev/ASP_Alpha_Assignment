
using Data.Repositories;
using Business.Models;
using Data.Entities;
using Data.Models;


namespace Business.Services;

public interface IClientService 
{
    Task<ClientResult> AddClientAsync(string clientName);
}
public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<ClientResult> AddClientAsync(string clientName)
    {
        if (string.IsNullOrWhiteSpace(clientName))
        {
            return new ClientResult
            {
                Succeeded = false,
                StatusCode = 400,
                Error = "Client name cannot be empty."
            };
        }

        var clientEntity = new ClientEntity
        {
            Id = Guid.NewGuid().ToString(),
            ClientName = clientName
        };
        var result = await _clientRepository.AddAsync(clientEntity);

        return result.Succeeded
            ? new ClientResult { Succeeded = true, StatusCode = 201, Result = new[] { new Client { Id = clientEntity.Id, ClientName = clientEntity.ClientName } } }
            : new ClientResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }
}
