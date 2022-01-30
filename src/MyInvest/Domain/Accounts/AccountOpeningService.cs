using MyInvest.Domain.Clients;

namespace MyInvest.Domain.Accounts;

public class AccountOpeningService
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
        VerifyClientCanOpenAccount(clientId, accountType);
        return CreateAccount(clientId, accountType);
    }

    private void VerifyClientCanOpenAccount(ClientId clientId, AccountType accountType)
    {
        if (!ClientExists(clientId))
        {
            throw new ClientDoesNotExistException("Client " + clientId.Value + " not found");
        }

        if (ClientOwnsAccountOfType(clientId, accountType))
        {
            throw new ClientAlreadyOwnsAccountException("Client " + clientId.Value + " already has an account of type " + accountType);
        }
    }

    private bool ClientExists(ClientId clientId) => _clientRepository.GetById(clientId) != null;

    private bool ClientOwnsAccountOfType(ClientId clientId, AccountType accountType) =>
        _accountRepository.FindByClientId(clientId).Any(account => account.AccountType == accountType);

    private InvestmentAccount CreateAccount(ClientId clientId, AccountType accountType)
    {
        _logger.LogInformation("Opening account of type {AccountType} for client {ClientId}", accountType, clientId);
        var newAccount = _accountFactory.NewAccount(clientId, accountType);
        _accountRepository.Create(newAccount);
        return newAccount;
    }
}
