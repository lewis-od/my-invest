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

    [Given(@"a client exists")]
    public void GivenAClientExists()
    {
        var username = RandomUsername();
        _scenarioContext[Username] = username;
        var clientId = _driver.CreateClient(username);
        _scenarioContext[ClientId] = clientId;
    }

    [Given(@"I have a MyInvest profile")]
    public async Task GivenIHaveAMyInvestProfile()
    {
        var username = RandomUsername();
        _scenarioContext[Username] = username;
        _scenarioContext[ClientId] = await _driver.SignUpAsync(username);
    }

    private string RandomUsername() => $"{_faker.Hacker.Adjective()}-{_faker.Hacker.Noun()}";

    [Given("my username is (.*)")]
    public void GivenMyUsernameIs(string username)
    {
        _scenarioContext[Username] = username;
    }

    [When("I sign up")]
    public async Task WhenISignUp()
    {
        var username = _scenarioContext.Get<string>(Username);
        _scenarioContext[ClientId] = await _driver.SignUpAsync(username);
    }

    [Then(@"I get assigned a user ID")]
    public async Task ThenIGetAssignedAUserId()
    {
        var client = await FetchClientAsync();
        client.ClientId.Should().NotBe(Guid.Empty);
    }

    [Then("I have (\\d*) investment accounts")]
    public async Task ThenIHaveDInvestmentAccounts(int numAccounts)
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
