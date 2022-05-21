using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MyInvest.ComponentTests.Drivers;
using MyInvest.Persistence;
using MyInvest.Persistence.Accounts;
using MyInvest.REST.Accounts;
using NUnit.Framework;
using static MyInvest.ComponentTests.Steps.ClientStepDefinitions;

namespace MyInvest.ComponentTests.Steps;

[Binding]
public class AccountStepDefinitions
{
    public const string AccountId = "accountId";

    private readonly ScenarioContext _scenarioContext;
    private readonly AccountDriver _accountDriver;

    public AccountStepDefinitions(ScenarioContext scenarioContext, RestClient restClient, IServiceScope scenarioScope)
    {
        _scenarioContext = scenarioContext;
        var accountDao = scenarioScope.ServiceProvider.GetRequiredService<IInvestmentAccountDao>();
        var dbContext = scenarioScope.ServiceProvider.GetRequiredService<MyInvestDbContext>();
        _accountDriver = new AccountDriver(restClient, accountDao, dbContext);
    }

    [Given(@"they have an? ([A-Z]*) account with status ([a-zA-Z]*)")]
    public void GivenTheyHaveAnXAccountWithStatusY(string accountType, string accountStatus)
    {
        var clientId = _scenarioContext.Get<Guid>(ClientId);
        var accountId = _accountDriver.CreateAccountForClient(clientId, accountType, accountStatus);
        _scenarioContext[AccountId] = accountId;
    }

    [When(@"I open a ([A-Z]*) account")]
    public async Task WhenIOpenAnInvestmentAccount(string accountType)
    {
        var clientId = _scenarioContext.Get<Guid>(ClientId);
        _scenarioContext[AccountId] = await _accountDriver.CreateAccountAsync(clientId, accountType);
    }

    [Then(@"an account with type ([A-Z]*) is created")]
    public async Task ThenAnAccountWithTypeXIsCreated(string accountType)
    {
        var createdAccount = await FetchAccountAsync();
        var createdAccountType = createdAccount.AccountType;
        Enum.GetName(typeof(AccountTypeDto), createdAccountType).Should().BeEquivalentTo(accountType);
    }

    [Then(@"the account is assigned an ID")]
    public async Task ThenTheAccountIsAssignedAnId()
    {
        var createdAccount = await FetchAccountAsync();
        createdAccount.AccountId.Should().NotBeEmpty();
    }

    [Then(@"the account has status ([a-zA-Z]*)")]
    public async Task ThenTheAccountHasStatusX(string accountStatus)
    {
        var createdAccount = await FetchAccountAsync();
        var createdAccountStatus = createdAccount?.Status;
        Assert.NotNull(createdAccountStatus);
        Enum.GetName(typeof(AccountStatusDto), createdAccountStatus).Should().BeEquivalentTo(accountStatus);
    }

    [Then(@"the account has balance £(\d*\.\d\d)")]
    public async Task ThenTheAccountHasBalanceD(decimal balance)
    {
        var createdAccount = await FetchAccountAsync();
        createdAccount.Balance.Should().Be(balance);
    }

    private async Task<AccountDto> FetchAccountAsync()
    {
        var clientId = _scenarioContext.Get<Guid>(ClientId);
        var accountId = _scenarioContext.Get<Guid>(AccountId);
        var account = await _accountDriver.FetchAccountAsync(clientId, accountId);
        Assert.NotNull(account);
        return account!;
    }
}
