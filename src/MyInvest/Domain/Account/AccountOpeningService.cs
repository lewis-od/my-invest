using MyInvest.Domain.Client;

namespace MyInvest.Domain.Account;

public class AccountOpeningService
{
    private readonly AccountFactory _accountFactory;
    private readonly IAccountRepository _accountRepository;

    public AccountOpeningService(AccountFactory accountFactory, IAccountRepository accountRepository)
    {
        _accountFactory = accountFactory;
        _accountRepository = accountRepository;
    }

    public InvestmentAccount OpenAccount(ClientId clientId, AccountType accountType)
    {
        if (ClientOwnsAccountOfType(clientId, accountType))
        {
            throw new ClientAlreadyOwnsAccountException();
        }
        
        var newAccount = _accountFactory.NewAccount(clientId, accountType);
        _accountRepository.Create(newAccount);
        return newAccount;
    }

    private bool ClientOwnsAccountOfType(ClientId clientId, AccountType accountType) =>
        _accountRepository.FindByClientId(clientId).Any(account => account.AccountType == accountType);
}
