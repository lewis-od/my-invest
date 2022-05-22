using MyInvest.Domain.Accounts;
using MyInvest.Persistence;
using MyInvest.Persistence.Accounts;
using MyInvest.REST.Accounts;
using MyInvest.REST.Clients;
using NUnit.Framework;

namespace MyInvest.ComponentTests.Drivers;

public class AccountDriver
{
    private readonly RestClient _restClient;
    private readonly IInvestmentAccountDao _accountDao;
    private readonly MyInvestDbContext _dbContext;

    public AccountDriver(RestClient restClient, IInvestmentAccountDao accountDao, MyInvestDbContext dbContext)
    {
        _restClient = restClient;
        _accountDao = accountDao;
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateAccountAsync(Guid clientId, string accountType)
    {
        var openAccountRequest = new OpenAccountRequestDto
        {
            ClientId = clientId
        };
        var endpoint = $"/accounts/open/{accountType.ToLower()}";
        var result = await _restClient.PostObjectAsync<OpenAccountRequestDto, AccountDto>(endpoint, openAccountRequest);
        Assert.IsTrue(result.HasSuccessStatusCode());
        Assert.NotNull(result.Body);
        return result.Body!.AccountId;
    }

    public async Task<RestResponse> AddCashToAccountAsync(Guid accountId, decimal amount)
    {
        var addCashRequest = new AddCashRequestDto
        {
            TransactionId = Guid.NewGuid(),
            MAC = "mac",
            Amount = amount
        };
        var endpoint = $"/accounts/{accountId}/add-cash";
        return await _restClient.PatchObjectAsync(endpoint, addCashRequest);
    }

    public Guid CreateAccountForClient(Guid clientId, string accountType, string status)
    {
        var entity = new InvestmentAccountEntity
        {
            AccountId = Guid.NewGuid(),
            AccountStatus = status,
            AccountType = accountType,
            Balance = 0.0m,
            ClientId = clientId
        };
        _dbContext.Database.BeginTransaction();
        _accountDao.CreateAccount(entity);
        _dbContext.Database.CommitTransaction();
        return entity.AccountId;
    }

    public async Task<RestResponse<AccountDto>> FetchAccountAsync(Guid accountId)
    {
        var endpoint = $"/accounts/{accountId}";
        return await _restClient.GetObjectAsync<AccountDto>(endpoint);
    }
}
