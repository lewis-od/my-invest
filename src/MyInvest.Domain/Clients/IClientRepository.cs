namespace MyInvest.Domain.Clients;

public interface IClientRepository
{
    public void Create(Client client);
    void Update(Client client);
    public Client? GetById(ClientId clientId);
    public bool IsUsernameTaken(string username);
}