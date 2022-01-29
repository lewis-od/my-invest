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

    public InvestmentAccount CreateAccount(AccountId accountId, Guid clientId, AccountType type, AccountStatus accountStatus, decimal balance) =>
        type switch
        {
            AccountType.ISA => CreateIsa(accountId, clientId, accountStatus, balance),
            AccountType.JISA => CreateJisa(accountId, clientId, accountStatus, balance),
            _ => CreateInvestmentAccount(accountId, clientId, type, accountStatus, balance),
        };
    
    public InvestmentAccount NewAccount(Guid clientId, AccountType type)
    {
        var newAccountId = _idGenerator.Generate();
        var accountStatus = AccountStatus.PreOpen;
        return type switch
        {
            AccountType.ISA => CreateIsa(newAccountId, clientId, accountStatus, InitialBalance),
            AccountType.JISA => CreateJisa(newAccountId, clientId, accountStatus, InitialBalance),
            _ => CreateInvestmentAccount(newAccountId, clientId, type, accountStatus, InitialBalance),
        };
    }

    private static SavingsAccount CreateIsa(AccountId accountId, Guid clientId, AccountStatus accountStatus, decimal balance) =>
        new(accountId, clientId, AccountType.ISA, accountStatus, balance, IsaSavingsAllowance);

    private static SavingsAccount CreateJisa(AccountId accountId, Guid clientId, AccountStatus accountStatus, decimal balance) =>
        new(accountId, clientId, AccountType.JISA, accountStatus, balance, JisaSavingsAllowance);

    private static InvestmentAccount CreateInvestmentAccount(
        AccountId accountId,
        Guid clientId,
        AccountType accountType,
        AccountStatus accountStatus,
        decimal balance
    ) => new(accountId, clientId, accountType, accountStatus, balance);
}