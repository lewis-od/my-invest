using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;

namespace MyInvest.Persistence.Accounts;

public class InvestmentAccountRepository : IAccountRepository
{
    private readonly IInvestmentAccountDao _dao;
    private readonly IInvestmentAccountEntityMapper _accountMapper;
    
    public InvestmentAccountRepository(IInvestmentAccountDao dao, IInvestmentAccountEntityMapper accountMapper)
    {
        _dao = dao;
        _accountMapper = accountMapper;
    }

    public IEnumerable<InvestmentAccount> GetAll() => _dao.GetAll()
        .Select(entity => _accountMapper.MapFromEntity(entity));

    public InvestmentAccount? GetById(AccountId accountId)
    {
        var accountEntity = _dao.GetById(accountId);
        return accountEntity == null ? null : _accountMapper.MapFromEntity(accountEntity);
    }

    public IEnumerable<InvestmentAccount> FindByClientId(ClientId clientId) => _dao.FindByClientId(clientId)
        .Select(entity => _accountMapper.MapFromEntity(entity));

    public void Create(InvestmentAccount newAccount)
    {
        var accountEntity = _accountMapper.MapToEntity(newAccount);
        _dao.CreateAccount(accountEntity);
    }

    public void Update(InvestmentAccount account)
    {
        var entity = _accountMapper.MapToEntity(account);
        _dao.UpdateAccount(entity);
    }
}
