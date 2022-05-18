using BoDi;
using Microsoft.Extensions.DependencyInjection;

namespace MyInvest.ComponentTests.Hooks;

[Binding]
public class WebServerHook
{
    private readonly IObjectContainer _objectContainer;

    public WebServerHook(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }
    
    [BeforeScenario]
    public void StartServer()
    {
        var application = new MyInvestApplicationFactory().WithWebHostBuilder(_ => { });
        _objectContainer.RegisterInstanceAs(application);

        var restClient = new RestClient(application.CreateClient());
        _objectContainer.RegisterInstanceAs(restClient);

        var scenarioScope = application.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        _objectContainer.RegisterInstanceAs(scenarioScope);
    }
}
