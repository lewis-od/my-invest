using MyInvest.Domain.Client;

namespace MyInvest.Domain.Account;

public class AccountOpeningService
{
    private readonly AccountFactory _accountFactory;
    private readonly IAccountRepository _accountRepository;
    private readonly IClientRepository _clientRepository;

    public AccountOpeningService(AccountFactory accountFactory, IAccountRepository accountRepository, IClientRepository clientRepository)
    {
        _accountFactory = accountFactory;
        _accountRepository = accountRepository;
        _clientRepository = clientRepository;
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
            throw new ClientNotFoundException("Client " + clientId.Value + " not found");
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
        var newAccount = _accountFactory.NewAccount(clientId, accountType);
        _accountRepository.Create(newAccount);
        return newAccount;
    }
}
