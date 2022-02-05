using System;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;

namespace MyInvest.UnitTests.Domain.Accounts;

public static class TestAccountFactory
{
    private static readonly AccountFactory AccountFactory = new(new AccountIdGenerator());

    public static InvestmentAccount NewAccount(ClientId clientId, AccountType accountType) => AccountFactory.NewAccount(clientId, accountType);

    public static InvestmentAccount NewAccount(Guid clientId, AccountType accountType) => AccountFactory.NewAccount(clientId, accountType);
}
