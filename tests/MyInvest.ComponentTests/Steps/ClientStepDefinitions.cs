using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MyInvest.ComponentTests.Drivers;
using MyInvest.REST.Clients;

namespace MyInvest.ComponentTests.Steps;

[Binding]
public sealed class ClientStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;
    private readonly ClientDriver _driver;

    private string _username = "";
    private ClientDto? _client = null;

    public ClientStepDefinitions(ScenarioContext scenarioContext, WebApplicationFactory<Program> application)
    {
        _scenarioContext = scenarioContext;
        _driver = new ClientDriver(application.CreateClient());
    }
    
    [Given("my username is (.*)")]
    public void GivenMyUsernameIs(string username)
    {
        _username = username;
    }

    [When("I sign up")]
    public async Task WhenISignUp()
    {
        _client = await _driver.SignUpAsync(_username);
    }
    
    [Then(@"I get assigned a user ID")]
    public void ThenIGetAssignedAUserId()
    {
        _client?.ClientId.Should().NotBe(Guid.Empty);
    }

    [Then("I have (\\d*) investment accounts")]
    public void ThenIHaveDInvestmentAccounts(int numAccounts)
    {
        _client?.InvestmentAccounts.Should().HaveCount(numAccounts);
    }
}
