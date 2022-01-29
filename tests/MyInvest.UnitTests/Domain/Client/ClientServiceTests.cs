using System;
using System.Linq;
using MyInvest.Domain.Account;
using MyInvest.Domain.Client;
using MyInvest.Domain.Id;
using MyInvest.UnitTests.Domain.Id;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace MyInvest.UnitTests.Domain.Client;

public class ClientServiceTests
{
    private readonly ClientId _clientId = ClientId.From(Guid.NewGuid());
    
    private readonly Mock<IClientRepository> _clientRepository;
    private readonly IUniqueIdGenerator<ClientId> _clientIdGenerator;

    private readonly ClientService _clientService;

    public ClientServiceTests()
    {
        _clientIdGenerator = new FixedIdGenerator<ClientId>(_clientId);
        _clientRepository = new Mock<IClientRepository>();
        _clientService = new ClientService(_clientRepository.Object, _clientIdGenerator);
    }

    [Test]
    public void CreatesNewClient()
    {
        const string username = "lewis";

        var newClient = _clientService.SignUp(username);

        var expectedClient = new MyInvest.Domain.Client.Client(_clientId, username, Enumerable.Empty<InvestmentAccount>());
        newClient.Should().BeEquivalentTo(expectedClient);
        _clientRepository.Verify(o => o.Save(newClient));
    }
}