using Microsoft.Extensions.Logging;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Ids;

namespace MyInvest.Domain.Clients;

public class OnboardingService
{
    private readonly IClientRepository _clientRepository;
    private readonly IUniqueIdGenerator<ClientId> _clientIdGenerator;
    private readonly ILogger<OnboardingService> _logger;

    public OnboardingService(IClientRepository clientRepository, IUniqueIdGenerator<ClientId> clientIdGenerator, ILogger<OnboardingService> logger)
    {
        _clientRepository = clientRepository;
        _clientIdGenerator = clientIdGenerator;
        _logger = logger;
    }

    public Client SignUp(string username, PostalAddress postalAddress)
    {
        if (_clientRepository.IsUsernameTaken(username))
        {
            throw new UsernameTakenException($"Username '{username}' is already in use.");
        }
        
        var clientId = _clientIdGenerator.Generate();
        var newClient = new Client(clientId, username, postalAddress, Enumerable.Empty<InvestmentAccount>());
        _logger.LogInformation("Creating new client with ID {ClientId}", clientId);
        _clientRepository.Create(newClient);
        return newClient;
    }
}
