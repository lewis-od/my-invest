using System.Net;
using FluentAssertions;
using NUnit.Framework;
using static MyInvest.ComponentTests.Steps.ScenarioContextKeys;

namespace MyInvest.ComponentTests.Steps;

[Binding]
public sealed class RestStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;

    public RestStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Then("the request is successful")]
    public void ThenTheRequestIsSuccessful()
    {
        var statusCode = _scenarioContext.Get<HttpStatusCode>(StatusCode);
        Assert.IsTrue(statusCode.IsSuccess());
    }
    
    [Then(@"they receive a conflict error code")]
    public void ThenTheyReceiveAConflictErrorCode()
    {
        var actualStatusCode = _scenarioContext.Get<HttpStatusCode>(StatusCode);
        actualStatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}

