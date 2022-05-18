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

public class OnboardingServiceTests
{
    private static readonly ClientId ClientId = ClientId.From(Guid.NewGuid());
    private static readonly PostalAddress Address = new("line1", "line2", "postcode");

    private readonly Mock<IClientRepository> _clientRepository;

    private readonly OnboardingService _onboardingService;

    public OnboardingServiceTests()
    {
        _clientRepository = new Mock<IClientRepository>();
        var clientIdGenerator = new FixedIdGenerator<ClientId>(ClientId);
        _onboardingService = new OnboardingService(_clientRepository.Object, clientIdGenerator, Mock.Of<ILogger<OnboardingService>>());
    }

    [Test]
    public void CreatesNewClient()
    {
        const string username = "lewis";
        _clientRepository.Setup(repo => repo.IsUsernameTaken(username)).Returns(false);

        var newClient = _onboardingService.SignUp(username, Address);

        var expectedClient = new Client(ClientId, username, Address, Enumerable.Empty<InvestmentAccount>());
        newClient.Should().BeEquivalentTo(expectedClient);
        _clientRepository.Verify(o => o.Create(newClient));
    }

    [Test]
    public void ThrowsExceptionIfUsernameInUse()
    {
        const string username = "lewis";
        _clientRepository.Setup(repo => repo.IsUsernameTaken(username)).Returns(true);

        Assert.Throws<UsernameTakenException>(() => _onboardingService.SignUp(username, Address));
    }
}