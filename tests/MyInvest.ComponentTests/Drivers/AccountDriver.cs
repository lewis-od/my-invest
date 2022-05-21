using MyInvest.Persistence;
using MyInvest.Persistence.Accounts;
using MyInvest.REST.Accounts;
using MyInvest.REST.Clients;

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

    public async Task<AccountDto?> CreateAccountAsync(Guid clientId, string accountType)
    {
        var openAccountRequest = new OpenAccountRequestDto
        {
            ClientId = clientId
        };
        var endpoint = $"/accounts/open/{accountType.ToLower()}";
        var result = await _restClient.PostObjectAsync<OpenAccountRequestDto, AccountDto>(endpoint, openAccountRequest);
        return result;
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

    public async Task<AccountDto?> FetchAccountAsync(Guid clientId, Guid accountId)
    {
        var endpoint = $"/clients/{clientId}";
        var result = await _restClient.GetObjectAsync<ClientDto>(endpoint);
        return result?.InvestmentAccounts.First(account => account.AccountId == accountId);
    }
}
