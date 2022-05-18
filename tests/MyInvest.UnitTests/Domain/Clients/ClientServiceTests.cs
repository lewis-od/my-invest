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
    private static readonly ClientId ClientId = ClientId.From(Guid.NewGuid());
    private static readonly PostalAddress Address = new("line1", "line2", "postcode");

    private readonly Mock<IClientRepository> _clientRepository;

    private readonly ClientService _clientService;

    public ClientServiceTests()
    {
        _clientRepository = new Mock<IClientRepository>();
        var clientIdGenerator = new FixedIdGenerator<ClientId>(ClientId);
        _clientService = new ClientService(_clientRepository.Object, clientIdGenerator, Mock.Of<ILogger<ClientService>>());
    }

    [Test]
    public void CreatesNewClient()
    {
        const string username = "lewis";
        _clientRepository.Setup(repo => repo.IsUsernameTaken(username)).Returns(false);

        var newClient = _clientService.SignUp(username, Address);

        var expectedClient = new Client(ClientId, username, Address, Enumerable.Empty<InvestmentAccount>());
        newClient.Should().BeEquivalentTo(expectedClient);
        _clientRepository.Verify(o => o.Create(newClient));
    }

    [Test]
    public void ThrowsExceptionIfUsernameInUse()
    {
        const string username = "lewis";
        _clientRepository.Setup(repo => repo.IsUsernameTaken(username)).Returns(true);

        Assert.Throws<UsernameTakenException>(() => _clientService.SignUp(username, Address));
    }
}