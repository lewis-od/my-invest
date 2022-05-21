using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MyInvest.ComponentTests.Drivers;
using MyInvest.Persistence;
using MyInvest.Persistence.Clients;
using MyInvest.REST.Clients;

namespace MyInvest.ComponentTests.Steps;

[Binding]
public sealed class ClientStepDefinitions
{
    public const string Username = "username";
    public const string ClientId = "clientId";
    public const string Client = "client";
    
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
        _scenarioContext[Client] = await _driver.SignUpAsync(username);
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
        _scenarioContext[Client] = await _driver.SignUpAsync(_scenarioContext.Get<string>("username"));
    }
    
    [Then(@"I get assigned a user ID")]
    public void ThenIGetAssignedAUserId()
    {
        _scenarioContext.Get<ClientDto>(Client).ClientId.Should().NotBe(Guid.Empty);
    }

    [Then("I have (\\d*) investment accounts")]
    public void ThenIHaveDInvestmentAccounts(int numAccounts)
    {
        _scenarioContext.Get<ClientDto>(Client).InvestmentAccounts.Should().HaveCount(numAccounts);
    }
}
