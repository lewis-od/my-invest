using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;

namespace MyInvest.Persistence.Clients;

public class ClientRepository : IClientRepository
{
    private readonly IClientDao _clientDao;
    private readonly IClientEntityMapper _entityMapper;
    private readonly IAccountRepository _accountRepository;

    public ClientRepository(IClientDao clientDao, IClientEntityMapper entityMapper, IAccountRepository accountRepository)
    {
        _clientDao = clientDao;
        _entityMapper = entityMapper;
        _accountRepository = accountRepository;
    }

    public void Create(Client client) => _clientDao.CreateClient(_entityMapper.MapToEntity(client));

    public void Update(Client client) => _clientDao.UpdateClient(_entityMapper.MapToEntity(client));

    public Client? GetById(ClientId clientId)
    {
        var entity = _clientDao.GetById(clientId.Value);
        var accounts = _accountRepository.FindByClientId(clientId);
        return entity != null ? _entityMapper.MapFromEntity(entity, accounts) : null;
    }

    public bool IsUsernameTaken(string username) => _clientDao.UsernameExists(username);
}
