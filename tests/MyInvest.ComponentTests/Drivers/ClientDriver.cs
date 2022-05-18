using MyInvest.Persistence.Clients;
using MyInvest.REST.Clients;

namespace MyInvest.ComponentTests.Drivers;

public class ClientDriver
{
    private readonly RestClient _restClient;
    private readonly IClientDao _clientDao;

    public ClientDriver(HttpClient httpClient, IClientDao clientDao)
    {
        _restClient = new RestClient(httpClient);
        _clientDao = clientDao;
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

    public Guid CreateClient(string username)
    {
        var entity = new ClientEntity
        {
            ClientId = Guid.NewGuid(),
            Username = username,

        };
        _clientDao.CreateClient(entity);
        return entity.ClientId;
    }
}
