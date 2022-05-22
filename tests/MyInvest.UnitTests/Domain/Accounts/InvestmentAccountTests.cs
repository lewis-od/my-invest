using System;
using FluentAssertions;
using MyInvest.Domain.Accounts;
using NUnit.Framework;

namespace MyInvest.UnitTests.Domain.Accounts;

public class InvestmentAccountTests
{
    [Test]
    public void AddsCreditAmountToBalance()
    {
        var account = TestAccountFactory.InvestmentAccountWithStatus(AccountStatus.Open);

        account.CreditBalance(12.34m);

        account.Balance.Should().Be(12.34m);
    }

    [Test]
    public void AddsCreditAmountToBalanceMultipleTimes()
    {
        var account = TestAccountFactory.InvestmentAccountWithStatus(AccountStatus.Open);

        account.CreditBalance(12.34m);
        account.CreditBalance(10.00m);

        account.Balance.Should().Be(22.34m);
    }

    [Test]
    public void CreditBalanceThrowsExceptionWhenAccountIsNotOpen(
        [Values(AccountStatus.PreOpen, AccountStatus.Closed)] AccountStatus accountStatus
    )
    {
        var account = TestAccountFactory.InvestmentAccountWithStatus(accountStatus);

        Assert.Throws<AccountNotOpenException>(() => account.CreditBalance(12.34m));
    }
}
