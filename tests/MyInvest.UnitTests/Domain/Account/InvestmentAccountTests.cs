using System;
using MyInvest.Domain.Account;
using MyInvest.Domain.Id;
using FluentAssertions;
using NUnit.Framework;

namespace MyInvest.UnitTests.Domain.Account;

public class InvestmentAccountTests
{
    private readonly InvestmentAccount _account = new(AccountId.From(Guid.NewGuid()), Guid.NewGuid(), AccountType.GIA, 0.0m);

    [Test]
    public void AddsCreditAmountToBalance()
    {
        _account.CreditBalance(12.34m);
        
        _account.Balance.Should().Be(12.34m);
    }

    [Test]
    public void AddsCreditAmountToBalanceMultipleTimes()
    {
        _account.CreditBalance(12.34m);
        _account.CreditBalance(10.00m);
        
        _account.Balance.Should().Be(22.34m);
    }
}