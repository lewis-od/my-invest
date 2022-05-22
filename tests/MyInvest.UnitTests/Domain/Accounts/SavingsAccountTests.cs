using System;
using MyInvest.Domain.Accounts;
using NUnit.Framework;

namespace MyInvest.UnitTests.Domain.Accounts;

public class SavingsAccountTests
{
    [Test]
    public void CreditBalanceUpdatesContributionAmount()
    {
        const decimal amount = 12.34m;
        var account = TestAccountFactory.SavingsAccountWithStatus(AccountStatus.Open);

        account.CreditBalance(amount);
        
        Assert.That(account.SavingsContributions, Is.EqualTo(amount));
        Assert.That(account.Balance, Is.EqualTo(amount));
    }

    [Test]
    public void CreditBalanceThrowsExceptionWhenAccountStatusIsNotOpen(
        [Values(AccountStatus.PreOpen, AccountStatus.Closed)] AccountStatus accountStatus
    )
    {
        var account = TestAccountFactory.SavingsAccountWithStatus(accountStatus);

        Assert.Throws<AccountNotOpenException>(() => account.CreditBalance(12.34m));
    }

    [Test]
    public void AddCashThrowsExceptionIfContributionsExceedAllowance()
    {
        var account = TestAccountFactory.SavingsAccountWithStatus(AccountStatus.Open);
        
        Assert.Throws<SavingsAllowanceExceededException>(() => account.CreditBalance(20_000.01m));
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