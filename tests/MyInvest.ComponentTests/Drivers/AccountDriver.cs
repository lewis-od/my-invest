using MyInvest.Persistence.Accounts;
using MyInvest.REST.Accounts;

namespace MyInvest.ComponentTests.Drivers;

public class AccountDriver
{
    private readonly RestClient _restClient;
    private readonly IInvestmentAccountDao _accountDao;

    public AccountDriver(HttpClient httpClient, IInvestmentAccountDao accountDao)
    {
        _restClient = new RestClient(httpClient);
        _accountDao = accountDao;
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
        _accountDao.CreateAccount(entity);
        return entity.AccountId;
    }
}
