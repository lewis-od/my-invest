using System;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;
using MyInvest.Domain.Ids;

namespace MyInvest.UnitTests.Domain.Accounts;

public static class TestAccountFactory
{
    private static readonly IUniqueIdGenerator<AccountId> IdGenerator = new AccountIdGenerator();
    private static readonly AccountFactory AccountFactory = new(IdGenerator);

    public static InvestmentAccount NewAccount() => NewAccount(Guid.NewGuid(), AccountType.GIA);

    public static InvestmentAccount NewAccount(ClientId clientId, AccountType accountType) => AccountFactory.NewAccount(clientId, accountType, false);

    public static InvestmentAccount NewAccount(Guid clientId, AccountType accountType) => AccountFactory.NewAccount(clientId, accountType, false);

    public static InvestmentAccount InvestmentAccountWithStatus(AccountStatus status) => AccountFactory.CreateAccount(
        IdGenerator.Generate(),
        Guid.NewGuid(),
        AccountType.GIA,
        status,
        0.0m
    );

    public static SavingsAccount SavingsAccountWithStatus(AccountStatus status) => (AccountFactory.CreateAccount(
        IdGenerator.Generate(),
        Guid.NewGuid(),
        AccountType.ISA,
        status,
        0.0m
    ) as SavingsAccount)!;
}
