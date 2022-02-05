using System;
using FluentAssertions;
using MyInvest.Persistence;
using MyInvest.Persistence.Clients;
using NUnit.Framework;

namespace MyInvest.IntegrationTests.Persistence.Clients;

public class ClientDaoTests
{
    private static readonly TestDatabase Database = new();

    private MyInvestDbContext _dbContext = null!;
    private ClientDao _dao = null!;

    [SetUp]
    public void SetUp()
    {
        _dbContext = Database.CreateContext();
        _dao = new ClientDao(_dbContext);

        _dbContext.Database.BeginTransaction();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.ChangeTracker.Clear();
        _dbContext.Dispose();
    }
    
    [Test]
    public void CreatesAndRetrievesClient()
    {
        var clientId = Guid.NewGuid();
        var entityToCreate = new ClientEntity {ClientId = clientId, Username = "billy"};
        _dao.CreateClient(entityToCreate);

        var retrievedEntity = _dao.GetById(clientId);
        retrievedEntity.Should().BeEquivalentTo(entityToCreate);
        
        _dbContext.ChangeTracker.Clear();
    }

    [Test]
    public void ReturnsTrueIfUsernameExists()
    {
        var entityToCreate = new ClientEntity {ClientId = Guid.NewGuid(), Username = "lewis"};
        _dao.CreateClient(entityToCreate);

        var lewisExists = _dao.UsernameExists("lewis");
        var mikeExists = _dao.UsernameExists("mike");
        
        Assert.IsTrue(lewisExists);
        Assert.IsFalse(mikeExists);
        
        _dbContext.ChangeTracker.Clear();
    }
}
