using MyInvest.Domain.Id;

namespace MyInvest.Domain.Account;

public class AccountFactory
{
    private const decimal InitialBalance = 0.00m;
    private const decimal IsaSavingsAllowance = 20_000.00m;
    private const decimal JisaSavingsAllowance = 9_000.00m;

    private readonly IUniqueIdGenerator<AccountId> _idGenerator;

    public AccountFactory(IUniqueIdGenerator<AccountId> idGenerator)
    {
        _idGenerator = idGenerator;
    }

    public InvestmentAccount CreateAccount(AccountId accountId, Guid clientId, AccountType type, decimal balance) =>
        type switch
        {
            AccountType.ISA => CreateIsa(accountId, clientId, balance),
            AccountType.JISA => CreateJisa(accountId, clientId, balance),
            _ => CreateInvestmentAccount(accountId, clientId, type, balance),
        };
    
    public InvestmentAccount NewAccount(Guid clientId, AccountType type)
    {
        var newAccountId = _idGenerator.Generate();
        return type switch
        {
            AccountType.ISA => CreateIsa(newAccountId, clientId, InitialBalance),
            AccountType.JISA => CreateJisa(newAccountId, clientId, InitialBalance),
            _ => CreateInvestmentAccount(newAccountId, clientId, type, InitialBalance),
        };
    }

    private static SavingsAccount CreateIsa(AccountId accountId, Guid clientId, decimal balance) =>
        new(accountId, clientId, AccountType.ISA, balance, IsaSavingsAllowance);

    private static SavingsAccount CreateJisa(AccountId accountId, Guid clientId, decimal balance) =>
        new(accountId, clientId, AccountType.JISA, balance, JisaSavingsAllowance);

    private static InvestmentAccount CreateInvestmentAccount(
        AccountId accountId,
        Guid clientId,
        AccountType accountType,
        decimal balance
    ) => new(accountId, clientId, accountType, balance);
}