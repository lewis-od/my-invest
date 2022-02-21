using System;
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
    public void CreatesAndRetrievesAccount()
    {
        var accountId = Guid.NewGuid();
        var accountToCreate = new InvestmentAccountEntity
        {
            AccountId = accountId,
            ClientId = Guid.NewGuid(),
            AccountStatus = "OPEN",
            AccountType = "GIA",
            Balance = 13.00m,
        };
        
        _dao.CreateAccount(accountToCreate);
        var retrievedAccount = _dao.GetById(accountId);

        retrievedAccount.Should().BeEquivalentTo(accountToCreate);
    }
}
