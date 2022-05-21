using MyInvest.ComponentTests.Drivers;
using static MyInvest.ComponentTests.Steps.ClientStepDefinitions;

namespace MyInvest.ComponentTests.Steps;

[Binding]
public class BackOfficeStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;
    private readonly BackOfficeDriver _driver;

    public BackOfficeStepDefinitions(ScenarioContext scenarioContext, RestClient restClient)
    {
        _scenarioContext = scenarioContext;
        _driver = new BackOfficeDriver(restClient);
    }
    
    [When(@"their address is verified by the back office team")]
    public async Task WhenTheirAddressIsVerifiedByTheBackOfficeTeam()
    {
        var clientId = _scenarioContext.Get<Guid>(ClientId);
        await _driver.VerifyAddressAsync(clientId);
    }
}