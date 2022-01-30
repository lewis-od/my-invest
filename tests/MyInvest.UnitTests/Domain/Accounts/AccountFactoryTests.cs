using System;
using System.Collections.Generic;
using FluentAssertions;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Ids;
using MyInvest.UnitTests.Domain.Ids;
using NUnit.Framework;

namespace MyInvest.UnitTests.Domain.Accounts;

public class AccountFactoryTests
{
    private const decimal Balance = 12.34m;
    private readonly AccountId _accountId = AccountId.From(Guid.NewGuid());
    private readonly Guid _clientId = Guid.NewGuid();
    private readonly AccountStatus _accountStatus = AccountStatus.Open;

    private readonly AccountFactory _accountFactory;

    public AccountFactoryTests()
    {
        IUniqueIdGenerator<AccountId> idGenerator = new FixedIdGenerator<AccountId>(_accountId);
        _accountFactory = new AccountFactory(idGenerator);
    }

    private static IEnumerable<TestCaseData> SavingsAccountTypes()
    {
        yield return new TestCaseData(AccountType.ISA, 20_000.00m);
        yield return new TestCaseData(AccountType.JISA, 9_000.00m);
    }
    
    [Test]
    [TestCaseSource(nameof(SavingsAccountTypes))]
    public void CreatesSavingsAccountsWithCorrectAllowances(AccountType accountType, decimal expectedAllowance)
    {
        var createdAccount = _accountFactory.CreateAccount(_accountId, _clientId, accountType, _accountStatus, Balance);
        
        var expectedAccount = new SavingsAccount(_accountId, _clientId, accountType, _accountStatus, Balance, expectedAllowance);
        createdAccount.Should().BeEquivalentTo(expectedAccount);
    }

    [Test]
    [TestCase(AccountType.GIA)]
    [TestCase(AccountType.SIPP)]
    public void CreatesOtherAccountTypes(AccountType accountType)
    {
        var createdAccount = _accountFactory.CreateAccount(_accountId, _clientId, accountType, _accountStatus, Balance);

        var expectedAccount = new InvestmentAccount(_accountId,  _clientId, accountType, _accountStatus, Balance);
        createdAccount.Should().BeEquivalentTo(expectedAccount);
    }
    
    [Test]
    [TestCaseSource(nameof(SavingsAccountTypes))]
    public void CreatesNewSavingsAccountsWithCorrectAllowances(AccountType accountType, decimal expectedAllowance)
    {
        var createdAccount = _accountFactory.NewAccount(_clientId, accountType);
        
        var expectedAccount = new SavingsAccount(_accountId, _clientId, accountType, AccountStatus.PreOpen, 0.00m, expectedAllowance);
        createdAccount.Should().BeEquivalentTo(expectedAccount);
    }

    [Test]
    [TestCase(AccountType.GIA)]
    [TestCase(AccountType.SIPP)]
    public void CreatesNewOtherAccountTypes(AccountType accountType)
    {
        var createdAccount = _accountFactory.NewAccount(_clientId, accountType);

        var expectedAccount = new InvestmentAccount(_accountId,  _clientId, accountType, AccountStatus.PreOpen, 0.00m);
        createdAccount.Should().BeEquivalentTo(expectedAccount);
    }
}