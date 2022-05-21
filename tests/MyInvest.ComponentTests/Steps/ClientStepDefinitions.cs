using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MyInvest.ComponentTests.Drivers;
using MyInvest.Persistence;
using MyInvest.Persistence.Clients;
using MyInvest.REST.Clients;
using NUnit.Framework;

namespace MyInvest.ComponentTests.Steps;

[Binding]
public sealed class ClientStepDefinitions
{
    public const string Username = "username";
    public const string ClientId = "clientId";

    private readonly ScenarioContext _scenarioContext;
    private readonly ClientDriver _driver;
    private readonly Faker _faker = new();

    public ClientStepDefinitions(ScenarioContext scenarioContext, RestClient restClient, IServiceScope scenarioScope)
    {
        _scenarioContext = scenarioContext;
        var clientDao = scenarioScope.ServiceProvider.GetRequiredService<IClientDao>();
        var dbContext = scenarioScope.ServiceProvider.GetRequiredService<MyInvestDbContext>();
        _driver = new ClientDriver(restClient, clientDao, dbContext);
    }

    [Given(@"a client has signed up")]
    public void GivenAClientHasSignedUp()
    {
        var username = RandomUsername();
        _scenarioContext[Username] = username;
        var clientId = _driver.CreateClient(username);
        _scenarioContext[ClientId] = clientId;
    }

    private string RandomUsername() => $"{_faker.Hacker.Adjective()}-{_faker.Hacker.Noun()}";

    [Given("a client has username (.*)")]
    public void GivenAClientHasUsername(string username)
    {
        _scenarioContext[Username] = username;
    }

    [When("they sign up for a profile")]
    public async Task WhenTheySignUpForAProfile()
    {
        var username = _scenarioContext.Get<string>(Username);
        _scenarioContext[ClientId] = await _driver.SignUpAsync(username);
    }

    [Then(@"they are assigned a user ID")]
    public async Task ThenTheyAreAssignedAUserId()
    {
        var client = await FetchClientAsync();
        client.ClientId.Should().NotBe(Guid.Empty);
    }

    [Then("they have (\\d*) investment accounts")]
    public async Task ThenTheyHaveDInvestmentAccounts(int numAccounts)
    {
        var client = await FetchClientAsync();
        client.InvestmentAccounts.Should().HaveCount(numAccounts);
    }

    private async Task<ClientDto> FetchClientAsync()
    {
        var clientId = _scenarioContext.Get<Guid>(ClientId);
        var client = await _driver.FetchClientAsync(clientId);
        Assert.NotNull(client);
        return client!;
    }
}
