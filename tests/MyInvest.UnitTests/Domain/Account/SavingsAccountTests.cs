using System;
using MyInvest.Domain.Account;
using MyInvest.Domain.Id;
using NUnit.Framework;

namespace MyInvest.UnitTests.Domain.Account;

public class SavingsAccountTests
{
    private readonly SavingsAccount _account = new(AccountId.From(Guid.NewGuid()), Guid.NewGuid(), AccountType.ISA,AccountStatus.Open, 0.0m, 20_000.00m);

    [Test]
    public void AddCashUpdatesContributionAmount()
    {
        var amount = 12.34m;
        
        _account.CreditBalance(amount);
        
        Assert.That(_account.SavingsContributions, Is.EqualTo(amount));
        Assert.That(_account.Balance, Is.EqualTo(amount));
    }

    [Test]
    public void AddCashThrowsExceptionIfContributionsExceedAllowance()
    {
        Assert.Throws<SavingsAllowanceExceededException>(() => _account.CreditBalance(20_000.01m));
    }

    [Test]
    public void ConstructorThrowsExceptionIfAccountTypeIsSipp()
    {
        Assert.Throws<ArgumentException>(() => new SavingsAccount(AccountId.From(Guid.NewGuid()), Guid.NewGuid(), AccountType.SIPP, AccountStatus.Open, 0.0m, 0.0m));
    }
    
    [Test]
    public void ConstructorThrowsExceptionIfAccountTypeIsGia()
    {
        Assert.Throws<ArgumentException>(() => new SavingsAccount(AccountId.From(Guid.NewGuid()), Guid.NewGuid(), AccountType.GIA, AccountStatus.Open, 0.0m, 0.0m));
    }
}