using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MyInvest.ComponentTests.Drivers;
using MyInvest.REST.Clients;

namespace MyInvest.ComponentTests.Steps;

[Binding]
public sealed class ClientStepDefinitions
{
    public const string Username = "username";
    public const string Client = "client";
    
    private readonly ScenarioContext _scenarioContext;
    private readonly ClientDriver _driver;
    private readonly Faker _faker = new();

    public ClientStepDefinitions(ScenarioContext scenarioContext, MyInvestApplicationFactory application)
    {
        _scenarioContext = scenarioContext;
        _driver = new ClientDriver(application.CreateClient());
    }
    
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

    [Given(@"I have a MyInvest profile")]
    public async Task GivenIHaveAMyInvestProfile()
    {
        var username = $"{_faker.Hacker.Adjective()}-{_faker.Hacker.Noun()}";
        _scenarioContext[Client] = await _driver.SignUpAsync(username);
    }
}
