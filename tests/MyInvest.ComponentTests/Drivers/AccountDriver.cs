using MyInvest.REST.Accounts;

namespace MyInvest.ComponentTests.Drivers;

public class AccountDriver
{
    private readonly RestClient _restClient;

    public AccountDriver(HttpClient httpClient)
    {
        _restClient = new RestClient(httpClient);
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
}
