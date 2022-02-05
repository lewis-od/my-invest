using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Moq;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;
using MyInvest.Persistence;
using MyInvest.Persistence.Clients;
using NUnit.Framework;

namespace MyInvest.UnitTests.Persistence.Clients;

public class ClientRepositoryTests
{
    private readonly Mock<IClientDao> _clientDao = new();

    private readonly ClientRepository _repository;

    public ClientRepositoryTests()
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<PersistenceMapperProfile>()).CreateMapper();
        var clientMapper = new ClientEntityMapper(mapper);
        _repository = new ClientRepository(_clientDao.Object, clientMapper);
    }

    [Test]
    public void CreatesNewClient()
    {
        var clientId = Guid.NewGuid();
        const string username = "lewis";
        var client = new Client(ClientId.From(clientId), username, Enumerable.Empty<InvestmentAccount>());
        
        _repository.Create(client);

        var expectedEntity = new ClientEntity {ClientId = clientId, Username = username};
        _clientDao.Verify(dao => dao.CreateClient(expectedEntity));
    }

    [Test]
    public void ReturnsClientById()
    {
        var clientId = Guid.NewGuid();
        const string username = "lewis";
        var clientEntity = new ClientEntity {ClientId = clientId, Username = username};
        _clientDao.Setup(dao => dao.GetById(clientId)).Returns(clientEntity);

        var client = _repository.GetById(ClientId.From(clientId));

        var expectedClient = new Client(ClientId.From(clientId), username, Enumerable.Empty<InvestmentAccount>());
        client.Should().BeEquivalentTo(expectedClient);
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
