using System;
using System.Linq;
using FluentAssertions;
using Moq;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;
using MyInvest.Persistence.Clients;
using NUnit.Framework;

namespace MyInvest.UnitTests.Persistence.Clients;

public class ClientRepositoryTests
{
    private readonly Mock<IClientDao> _clientDao = new();
    private readonly Mock<IClientEntityMapper> _clientMapper = new();

    private readonly ClientRepository _repository;

    public ClientRepositoryTests()
    {
        _repository = new ClientRepository(_clientDao.Object, _clientMapper.Object);
    }

    [Test]
    public void CreatesNewClient()
    {
        var client = new Client(ClientId.From(Guid.NewGuid()), "lewis", Enumerable.Empty<InvestmentAccount>());
        var entity = new ClientEntity();
        _clientMapper.Setup(mapper => mapper.MapToEntity(client)).Returns(entity);
        
        _repository.Create(client);

        _clientDao.Verify(dao => dao.CreateClient(entity));
    }

    [Test]
    public void ReturnsClientById()
    {
        var clientId = Guid.NewGuid();
        var clientEntity = new ClientEntity();
        _clientDao.Setup(dao => dao.GetById(clientId)).Returns(clientEntity);
        var retrievedClient = new Client(ClientId.From(clientId), "lewis", Enumerable.Empty<InvestmentAccount>());
        _clientMapper.Setup(mapper => mapper.MapFromEntity(clientEntity)).Returns(retrievedClient);

        var client = _repository.GetById(ClientId.From(clientId));

        client.Should().BeEquivalentTo(retrievedClient);
    }

    [Test]
    public void ReturnsNullIfClientNotFoundForId()
    {
        var clientId = Guid.NewGuid();
        _clientDao.Setup(dao => dao.GetById(clientId)).Returns((ClientEntity?) null);

        var client = _repository.GetById(ClientId.From(clientId));
        
        Assert.IsNull(client);
    }
}
