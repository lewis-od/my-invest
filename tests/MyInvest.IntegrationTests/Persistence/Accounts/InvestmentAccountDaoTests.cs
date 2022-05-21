using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MyInvest.Persistence;
using MyInvest.Persistence.Accounts;
using NUnit.Framework;

namespace MyInvest.IntegrationTests.Persistence.Accounts;

public class InvestmentAccountDaoTests
{
    private static readonly TestDatabase Database = new();

    private MyInvestDbContext _dbContext = null!;
    private InvestmentAccountDao _dao = null!;

    [SetUp]
    public void SetUp()
    {
        _dbContext = Database.CreateContext();
        _dao = new InvestmentAccountDao(_dbContext);

        _dbContext.Database.BeginTransaction();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.ChangeTracker.Clear();
        _dbContext.Dispose();
    }

    [Test]
    public void CreatesAndRetrievesAccountByAccountId()
    {
        var accountId = Guid.NewGuid();
        var accountToCreate = NewAccount(accountId);

        _dao.CreateAccount(accountToCreate);
        _dbContext.ChangeTracker.Clear();

        var retrievedAccount = _dao.GetById(accountId);

        retrievedAccount.Should().BeEquivalentTo(accountToCreate);
    }

    [Test]
    public void CreatesAndRetrievesMultipleAccounts()
    {
        var account1 = NewAccount();
        _dao.CreateAccount(account1);
        var account2 = NewAccount();
        _dao.CreateAccount(account2);
        _dbContext.ChangeTracker.Clear();

        var retrievedAccounts = _dao.GetAll().ToList();

        retrievedAccounts.Should().HaveCount(2);
        retrievedAccounts.Should().Contain(new List<InvestmentAccountEntity> {account1, account2});
    }

    [Test]
    public void CreatesAndRetrievesAccountsByClientId()
    {
        var account1 = NewAccount();
        _dao.CreateAccount(account1);
        var clientId = Guid.NewGuid();
        var account2 = NewAccount(Guid.NewGuid(), clientId);
        _dao.CreateAccount(account2);
        _dbContext.ChangeTracker.Clear();

        var retrievedAccounts = _dao.FindByClientId(clientId).ToList();

        retrievedAccounts.Should().HaveCount(1);
        retrievedAccounts.Should().Contain(account2);
    }

    [Test]
    public void CreatesUpdatesAndRetrievesAccount()
    {
        var account = NewAccount();
        _dao.CreateAccount(account);
        _dbContext.ChangeTracker.Clear();

        account.Balance = 150.0m;
        _dao.UpdateAccount(account);
        _dbContext.ChangeTracker.Clear();

        // _dbContext.Database.BeginTransaction();
        var retrievedAccount = _dao.GetById(account.AccountId);
        retrievedAccount?.Balance.Should().Be(150.0m);
    }

    private static InvestmentAccountEntity NewAccount() => NewAccount(Guid.NewGuid());

    private static InvestmentAccountEntity NewAccount(Guid accountId, Guid? clientId = null) => new()
    {
        AccountId = accountId,
        ClientId = clientId ?? Guid.NewGuid(),
        AccountStatus = "Open",
        AccountType = "GIA",
        Balance = 13.00m,
    };
}
