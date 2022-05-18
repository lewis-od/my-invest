namespace MyInvest.ComponentTests.Drivers;

public class BackOfficeDriver
{
    private readonly RestClient _client;

    public BackOfficeDriver(HttpClient httpClient)
    {
        _client = new RestClient(httpClient);
    }

    public async Task VerifyAddressAsync(Guid clientId)
    {
        await _client.PutAsync($"/back-office/clients/{clientId}/address/verify");
    }
}
