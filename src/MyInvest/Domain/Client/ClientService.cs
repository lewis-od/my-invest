using MyInvest.Domain.Account;
using MyInvest.Domain.Id;

namespace MyInvest.Domain.Client;

public class ClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly IUniqueIdGenerator<ClientId> _clientIdGenerator;

    public ClientService(IClientRepository clientRepository, IUniqueIdGenerator<ClientId> clientIdGenerator)
    {
        _clientRepository = clientRepository;
        _clientIdGenerator = clientIdGenerator;
    }

    public Client SignUp(string username)
    {
        var clientId = _clientIdGenerator.Generate();
        var newClient = new Client(clientId, username, Enumerable.Empty<InvestmentAccount>());
        _clientRepository.Save(newClient);
        return newClient;
    }
}