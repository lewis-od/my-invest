using MyInvest.Domain.Clients;

namespace MyInvest.Persistence.Clients;

public class ClientRepository : IClientRepository
{
    private readonly ClientDao _clientDao;
    private readonly ClientEntityMapper _entityMapper;

    public ClientRepository(ClientDao clientDao, ClientEntityMapper entityMapper)
    {
        _clientDao = clientDao;
        _entityMapper = entityMapper;
    }

    public void Save(Client client) => _clientDao.CreateClient(_entityMapper.MapToEntity(client));

    public Client? GetById(ClientId clientId)
    {
        var entity = _clientDao.GetById(clientId.Value);
        return entity != null ? _entityMapper.MapFromEntity(entity) : null;
    }

    public bool IsUsernameTaken(string username) => _clientDao.UsernameExists(username);
}
