using System;
using System.Linq;
using FluentAssertions;
using Moq;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;
using MyInvest.Persistence.Clients;
using MyInvest.UnitTests.Domain.Accounts;
using NUnit.Framework;

namespace MyInvest.UnitTests.Persistence.Clients;

public class ClientRepositoryTests
{
    private static readonly PostalAddress Address = new("some", "dummy", "address");

    private readonly Mock<IClientDao> _clientDao = new();
    private readonly Mock<IClientEntityMapper> _clientMapper = new();
    private readonly Mock<IAccountRepository> _accountRepository = new();

    private readonly ClientRepository _clientRepository;

    public ClientRepositoryTests()
    {
        _clientRepository = new ClientRepository(_clientDao.Object, _clientMapper.Object, _accountRepository.Object);
    }

    [Test]
    public void CreatesNewClient()
    {
        var client = new Client(ClientId.From(Guid.NewGuid()), "lewis", Address, Enumerable.Empty<InvestmentAccount>());
        var entity = new ClientEntity();
        _clientMapper.Setup(mapper => mapper.MapToEntity(client)).Returns(entity);
        
        _clientRepository.Create(client);

        _clientDao.Verify(dao => dao.CreateClient(entity));
    }

    [Test]
    public void UpdatesExistingClient()
    {
        var client = new Client(ClientId.From(Guid.NewGuid()), "lewis", Address, Enumerable.Empty<InvestmentAccount>());
        var entity = new ClientEntity();
        _clientMapper.Setup(mapper => mapper.MapToEntity(client)).Returns(entity);
        
        _clientRepository.Update(client);

        _clientDao.Verify(dao => dao.UpdateClient(entity));
    }

    [Test]
    public void ReturnsClientById()
    {
        var clientId = Guid.NewGuid();
        var clientEntity = new ClientEntity();
        _clientDao.Setup(dao => dao.GetById(clientId)).Returns(clientEntity);

        var accounts = new[] {TestAccountFactory.NewAccount(clientId, AccountType.GIA)};
        _accountRepository.Setup(repo => repo.FindByClientId(ClientId.From(clientId))).Returns(accounts);
        
        var retrievedClient = new Client(ClientId.From(clientId), "lewis", Address, accounts);
        _clientMapper.Setup(mapper => mapper.MapFromEntity(clientEntity, accounts)).Returns(retrievedClient);

        var client = _clientRepository.GetById(ClientId.From(clientId));

        client.Should().BeEquivalentTo(retrievedClient);
    }

    [Test]
    public void ReturnsNullIfClientNotFoundForId()
    {
        var clientId = Guid.NewGuid();
        _clientDao.Setup(dao => dao.GetById(clientId)).Returns((ClientEntity?) null);

        var client = _clientRepository.GetById(ClientId.From(clientId));
        
        Assert.IsNull(client);
    }
}
