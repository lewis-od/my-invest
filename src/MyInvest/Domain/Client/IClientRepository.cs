namespace MyInvest.Domain.Client;

public interface IClientRepository
{
    public void Save(Client client);
    public Client? GetById(ClientId clientId);
}