using Microsoft.Extensions.Logging;
using MyInvest.Domain.Clients;

namespace MyInvest.Domain.Accounts;

public interface IAccountOpeningService
{
    InvestmentAccount OpenAccount(ClientId clientId, AccountType accountType);
}

public class AccountOpeningService : IAccountOpeningService
{
    private readonly AccountFactory _accountFactory;
    private readonly IAccountRepository _accountRepository;
    private readonly IClientRepository _clientRepository;
    private readonly ILogger<AccountOpeningService> _logger;

    public AccountOpeningService(
        AccountFactory accountFactory,
        IAccountRepository accountRepository,
        IClientRepository clientRepository,
        ILogger<AccountOpeningService> logger
    )
    {
        _accountFactory = accountFactory;
        _accountRepository = accountRepository;
        _clientRepository = clientRepository;
        _logger = logger;
    }

    public InvestmentAccount OpenAccount(ClientId clientId, AccountType accountType)
    {
        var client = FetchClientOrThrow(clientId);
        VerifyClientCanOpenAccount(client, accountType);
        return CreateAccount(client, accountType);
    }
    
    private Client FetchClientOrThrow(ClientId clientId)
    {
        var client = _clientRepository.GetById(clientId);
        if (client == null)
        {
            throw new ClientDoesNotExistException($"Client {clientId.Value} not found");
        }

        return client;
    }

    private void VerifyClientCanOpenAccount(Client client, AccountType accountType)
    {
        var clientId = client.ClientId;
        if (ClientOwnsAccountOfType(clientId, accountType))
        {
            throw new ClientAlreadyOwnsAccountException($"Client {clientId.Value} already has an account of type {accountType}");
        }
    }

    private bool ClientOwnsAccountOfType(ClientId clientId, AccountType accountType) =>
        _accountRepository.FindByClientId(clientId).Any(account => account.AccountType == accountType);

    private InvestmentAccount CreateAccount(Client client, AccountType accountType)
    {
        _logger.LogInformation("Opening account of type {AccountType} for client {ClientId}", accountType, client.ClientId);
        var newAccount = _accountFactory.NewAccount(client.ClientId, accountType, client.Address.IsVerified);
        _accountRepository.Create(newAccount);
        return newAccount;
    }
}
