using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;
using MyInvest.UnitTests.Domain.Ids;
using NUnit.Framework;

namespace MyInvest.UnitTests.Domain.Clients;

public class ClientServiceTests
{
    private readonly ClientId _clientId = ClientId.From(Guid.NewGuid());
    
    private readonly Mock<IClientRepository> _clientRepository;

    private readonly ClientService _clientService;

    public ClientServiceTests()
    {
        _clientRepository = new Mock<IClientRepository>();
        var clientIdGenerator = new FixedIdGenerator<ClientId>(_clientId);
        _clientService = new ClientService(_clientRepository.Object, clientIdGenerator, Mock.Of<ILogger<ClientService>>());
    }

    [Test]
    public void CreatesNewClient()
    {
        const string username = "lewis";
        _clientRepository.Setup(repo => repo.IsUsernameTaken(username)).Returns(false);

        var newClient = _clientService.SignUp(username);

        var expectedClient = new Client(_clientId, username, Enumerable.Empty<InvestmentAccount>());
        newClient.Should().BeEquivalentTo(expectedClient);
        _clientRepository.Verify(o => o.Save(newClient));
    }

    [Test]
    public void ThrowsExceptionIfUsernameInUse()
    {
        const string username = "lewis";
        _clientRepository.Setup(repo => repo.IsUsernameTaken(username)).Returns(true);

        Assert.Throws<UsernameTakenException>(() => _clientService.SignUp(username));
    }
}