using MyInvest.Persistence;
using MyInvest.Persistence.Clients;
using MyInvest.REST.Clients;
using NUnit.Framework;

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
    
    public async Task<RestResponse<ClientDto>> SignUpAsync(string username)
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
        return await _restClient.PostObjectAsync<SignUpRequestDto, ClientDto>("/clients/sign-up", createClientRequest);
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

    public void MarkAddressAsVerified(Guid clientId)
    {
        var client = _clientDao.GetById(clientId);
        Assert.NotNull(client);
        client!.AddressIsVerified = true;
        _dbContext.Database.BeginTransaction();
        _clientDao.UpdateClient(client);
        _dbContext.Database.CommitTransaction();
    }

    public async Task<RestResponse<ClientDto>> FetchClientAsync(Guid clientId)
    {
        var endpoint = $"/clients/{clientId}";
        return await _restClient.GetObjectAsync<ClientDto>(endpoint);
    }
}
