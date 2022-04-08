using MyInvest.REST.Clients;

namespace MyInvest.Specs.Drivers;

public class ClientDriver
{
    private readonly RestClient _restClient;

    public ClientDriver(string baseUrl)
    {
        _restClient = new RestClient(baseUrl);
    }
    
    public async Task<ClientDto?> SignUp(string username)
    {
        var createClientRequest = new SignUpRequestDto
        {
            Username = username
        };
        var result = await _restClient.PostObject<SignUpRequestDto, ClientDto>("/clients/sign-up", createClientRequest);
        return result;
    }
}
