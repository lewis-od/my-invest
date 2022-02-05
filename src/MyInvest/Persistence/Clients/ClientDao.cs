namespace MyInvest.Persistence.Clients;

public interface IClientDao
{
    void CreateClient(ClientEntity clientEntity);
    ClientEntity? GetById(Guid clientId);
    bool UsernameExists(string username);
}

public class ClientDao : IClientDao
{
    private readonly MyInvestDbContext _dbContext;

    public ClientDao(MyInvestDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void CreateClient(ClientEntity clientEntity)
    {
        _dbContext.Clients.Add(clientEntity);
        _dbContext.SaveChanges();
    }

    public ClientEntity? GetById(Guid clientId) => _dbContext.Clients
        .Where(client => client.ClientId == clientId)
        .ToList()
        .FirstOrDefault((ClientEntity?) null);

    public bool UsernameExists(string username) => _dbContext.Clients.Any(client => client.Username == username);
}
