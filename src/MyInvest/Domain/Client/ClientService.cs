using MyInvest.Domain.Account;
using MyInvest.Domain.Id;

namespace MyInvest.Domain.Client;

public class ClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly IUniqueIdGenerator<ClientId> _clientIdGenerator;
    private readonly ILogger<ClientService> _logger;

    public ClientService(IClientRepository clientRepository, IUniqueIdGenerator<ClientId> clientIdGenerator, ILogger<ClientService> logger)
    {
        _clientRepository = clientRepository;
        _clientIdGenerator = clientIdGenerator;
        _logger = logger;
    }

    public Client SignUp(string username)
    {
        var clientId = _clientIdGenerator.Generate();
        var newClient = new Client(clientId, username, Enumerable.Empty<InvestmentAccount>());
        _logger.LogInformation("Creating new client with ID {ClientId}", clientId);
        _clientRepository.Save(newClient);
        return newClient;
    }
}
