using MyInvest.Persistence;
using MyInvest.Persistence.Clients;
using MyInvest.REST.Clients;

namespace MyInvest.ComponentTests.Drivers;

public class ClientDriver
{
    private readonly RestClient _restClient;
    private readonly IClientDao _clientDao;
    private readonly MyInvestDbContext _dbContext;

    public ClientDriver(RestClient restClient, IClientDao clientDao, MyInvestDbContext dbContext)
    {
        _restClient = restClient;
        _clientDao = clientDao;
        _dbContext = dbContext;
    }
    
    public async Task<Guid?> SignUpAsync(string username)
    {
        var createClientRequest = new SignUpRequestDto
        {
            Username = username,
            Address = new PostalAddressDto
            {
                Line1 = "Line 1",
                Line2 = "Line 2",
                Postcode = "M15 4AB",
            }
        };
        var result = await _restClient.PostObjectAsync<SignUpRequestDto, ClientDto>("/clients/sign-up", createClientRequest);
        return result?.ClientId;
    }

    public Guid CreateClient(string username)
    {
        var entity = new ClientEntity
        {
            ClientId = Guid.NewGuid(),
            Username = username,
            AddressLine1 = "line1",
            AddressLine2 = "line2",
            AddressPostcode = "postcode",
            AddressIsVerified = false,
        };
        _dbContext.Database.BeginTransaction();
        _clientDao.CreateClient(entity);
        _dbContext.Database.CommitTransaction();
        return entity.ClientId;
    }

    public async Task<ClientDto?> FetchClientAsync(Guid clientId)
    {
        var endpoint = $"/clients/{clientId}";
        return await _restClient.GetObjectAsync<ClientDto>(endpoint);
    }
}
