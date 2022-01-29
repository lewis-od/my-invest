using MyInvest.Domain.Account;

namespace MyInvest.Persistence;

public class InMemoryAccountRepository : IAccountRepository
{
    private readonly List<InvestmentAccount> _accounts = new();
    private readonly AccountFactory _accountFactory;

    public InMemoryAccountRepository(AccountFactory accountFactory)
    {
        _accountFactory = accountFactory;
        
        SetupAccounts();
    }

    private void SetupAccounts()
    {
        var accountId1 = AccountId.From(Guid.Parse("bf9fd179-bf49-4c7c-b3f5-76d2fe6ee0cf"));
        var clientId1 = Guid.Parse("eedca41c-b4d9-4f21-a5ce-9b4a89692e2d");
        var account1 = _accountFactory.CreateAccount(accountId1, clientId1, AccountType.GIA, 123.00m);
        _accounts.Add(account1);
        var accountId2 = AccountId.From(Guid.Parse("98efcbe7-681a-46b6-ae5b-73dc70ef7a80"));
        var account2 = _accountFactory.CreateAccount(accountId2, clientId1, AccountType.SIPP, 8000.00m);
        _accounts.Add(account2);
        var accountId3 = AccountId.From(Guid.Parse("99908e75-130d-4043-8e82-1ff68731ccf8"));
        var clientId2 = Guid.Parse("74a140f4-32bb-42b2-b4d6-8e1eec0f4086");
        var account3 = _accountFactory.CreateAccount(accountId3, clientId2, AccountType.JISA, 3500.00m);
        _accounts.Add(account3);
    }

    public IEnumerable<InvestmentAccount> GetAll() => _accounts;

    public InvestmentAccount? GetById(AccountId accountId) => 
        _accounts.FirstOrDefault(account => account.AccountId.Equals(accountId));
}