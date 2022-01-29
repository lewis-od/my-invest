using MyInvest.Domain.Client;

namespace MyInvest.Persistence;

public class InMemoryClientRepository : IClientRepository
{
    private readonly IDictionary<ClientId, Client> _clients = new Dictionary<ClientId, Client>();

    public void Save(Client client)
    {
        _clients[client.ClientId] = client;
    }

    public Client? GetById(ClientId clientId) => _clients.ContainsKey(clientId) ? _clients[clientId] : null;
}