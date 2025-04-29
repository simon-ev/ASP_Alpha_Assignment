
using Data.Repositories;
using Data.Models;


namespace Data.Services;

public interface IClientService 
{
    Task<ClientResult> GetClientsAsync();
}
public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public Task<ClientResult> GetClientsAsync()
    {
        return _clientRepository.GetClientsAsync();
    }
}
