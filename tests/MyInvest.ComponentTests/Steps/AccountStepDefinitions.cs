using FluentAssertions;
using MyInvest.ComponentTests.Drivers;
using MyInvest.REST.Accounts;
using MyInvest.REST.Clients;
using static MyInvest.ComponentTests.Steps.ClientStepDefinitions;

namespace MyInvest.ComponentTests.Steps;

[Binding]
public class AccountStepDefinitions
{
    public const string Account = "account";
    
    private readonly ScenarioContext _scenarioContext;
    private readonly AccountDriver _accountDriver;

    public AccountStepDefinitions(ScenarioContext scenarioContext, MyInvestApplicationFactory applicationFactory)
    {
        _scenarioContext = scenarioContext;
        _accountDriver = new AccountDriver(applicationFactory.CreateClient());
    }

    [When(@"I open a ([A-Z]*) account")]
    public async Task WhenIOpenAnInvestmentAccount(string accountType)
    {
        var client = _scenarioContext.Get<ClientDto>(Client);
        _scenarioContext[Account] = await _accountDriver.CreateAccountAsync(client.ClientId, accountType);
    }
    
    [Then(@"an account with type ([A-Z]*) is created")]
    public void ThenAnAccountWithTypeXIsCreated(string accountType)
    {
        var createdAccount = _scenarioContext.Get<AccountDto>(Account);
        var createdAccountType = createdAccount.AccountType;
        Enum.GetName(typeof(AccountTypeDto), createdAccountType).Should().BeEquivalentTo(accountType);
    }
    
    [Then(@"the account is assigned an ID")]
    public void ThenTheAccountIsAssignedAnId()
    {
        var createdAccount = _scenarioContext.Get<AccountDto>(Account);
        createdAccount.AccountId.Should().NotBeEmpty();
    }
    
    [Then(@"the account has status ([a-zA-Z]*)")]
    public void ThenTheAccountHasStatusX(string accountStatus)
    {
        var createdAccount = _scenarioContext.Get<AccountDto>(Account);
        var createdAccountStatus = createdAccount.Status;
        Enum.GetName(typeof(AccountStatusDto), createdAccountStatus).Should().BeEquivalentTo(accountStatus);
    }

    [Then(@"the account has balance £(\d*\.\d\d)")]
    public void ThenTheAccountHasBalanceD(decimal balance)
    {
        var createdAccount = _scenarioContext.Get<AccountDto>(Account);
        createdAccount.Balance.Should().Be(balance);
    }
}
