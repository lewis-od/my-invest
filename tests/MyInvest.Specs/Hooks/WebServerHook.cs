using BoDi;

namespace MyInvest.Specs.Hooks;

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
    }
}
