using Microsoft.EntityFrameworkCore;

namespace MyInvest.Persistence.Clients;

public class ClientDao
{
    private readonly MyInvestDbContext _dbContext;

    public ClientDao(MyInvestDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual void CreateClient(ClientEntity clientEntity)
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
