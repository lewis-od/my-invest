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
        var entityToCreate = ClientWithIdAndUsername(clientId, "billy");
        _dao.CreateClient(entityToCreate);

        var retrievedEntity = _dao.GetById(clientId);
        retrievedEntity.Should().BeEquivalentTo(entityToCreate);
    }

    [Test]
    public void CreatesUpdatesAndRetrievesClient()
    {
        var clientId = Guid.NewGuid();
        var entity = ClientWithIdAndUsername(clientId, "billy");
        _dao.CreateClient(entity);

        entity.Username = "lewis";
        _dao.UpdateClient(entity);

        var retrievedEntity = _dao.GetById(clientId);
        Assert.NotNull(retrievedEntity);
        retrievedEntity!.Username.Should().Be("lewis");
    }

    [Test]
    public void ReturnsTrueIfUsernameExists()
    {
        var entityToCreate = ClientWithIdAndUsername(Guid.NewGuid(), "lewis");
        _dao.CreateClient(entityToCreate);

        var lewisExists = _dao.UsernameExists("lewis");
        var mikeExists = _dao.UsernameExists("mike");

        lewisExists.Should().BeTrue();
        mikeExists.Should().BeFalse();
    }

    private static ClientEntity ClientWithIdAndUsername(Guid clientId, string username) => new()
    {
        ClientId = clientId,
        Username = username,
        AddressLine1 = "line 1",
        AddressLine2 = "line 2",
        AddressPostcode = "postcode",
        AddressIsVerified = false
    };
}
