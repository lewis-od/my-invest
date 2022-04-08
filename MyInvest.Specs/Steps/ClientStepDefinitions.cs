﻿using FluentAssertions;
using MyInvest.REST.Clients;
using MyInvest.Specs.Drivers;

namespace MyInvest.Specs.Steps;

[Binding]
public sealed class ClientStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;
    private readonly ClientDriver _driver = new("http://localhost:5020");

    private string _username = "";
    private ClientDto? _client = null;

    public ClientStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }
    
    [Given("my username is (.*)")]
    public void GivenMyUsernameIs(string username)
    {
        _username = username;
    }

    [When("I sign up")]
    public async Task WhenISignUp()
    {
        _client = await _driver.SignUp(_username);
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