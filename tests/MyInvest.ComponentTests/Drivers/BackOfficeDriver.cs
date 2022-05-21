namespace MyInvest.ComponentTests.Drivers;

public class BackOfficeDriver
{
    private readonly RestClient _client;

    public BackOfficeDriver(RestClient client)
    {
        _client = client;
    }

    public async Task VerifyAddressAsync(Guid clientId)
    {
        await _client.PutAsync($"/back-office/clients/{clientId}/address/verify");
    }
}
