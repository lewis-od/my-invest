using MyInvest.REST.Clients;

namespace MyInvest.Specs.Drivers;

public class ClientDriver
{
    private readonly RestClient _restClient;

    public ClientDriver(HttpClient httpClient)
    {
        _restClient = new RestClient(httpClient);
    }
    
    public async Task<ClientDto?> SignUpAsync(string username)
    {
        var createClientRequest = new SignUpRequestDto
        {
            Username = username
        };
        var result = await _restClient.PostObjectAsync<SignUpRequestDto, ClientDto>("/clients/sign-up", createClientRequest);
        return result;
    }
}
