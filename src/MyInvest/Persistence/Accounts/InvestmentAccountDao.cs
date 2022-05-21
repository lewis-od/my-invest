namespace MyInvest.Persistence.Accounts;

public interface IInvestmentAccountDao
{
    void CreateAccount(InvestmentAccountEntity account);
    void UpdateAccount(InvestmentAccountEntity newAccountEntity);
    InvestmentAccountEntity? GetById(Guid accountId);
    IEnumerable<InvestmentAccountEntity> GetAll();
    IEnumerable<InvestmentAccountEntity> FindByClientId(Guid clientId);
}

public class InvestmentAccountDao : IInvestmentAccountDao
{
    private readonly MyInvestDbContext _dbContext;

    public InvestmentAccountDao(MyInvestDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void CreateAccount(InvestmentAccountEntity account)
    {
        _dbContext.InvestmentAccounts.Add(account);
        _dbContext.SaveChanges();
    }

    public void UpdateAccount(InvestmentAccountEntity account)
    {
        _dbContext.InvestmentAccounts.Update(account);
        _dbContext.SaveChanges();
    }

    public InvestmentAccountEntity? GetById(Guid accountId) => _dbContext.InvestmentAccounts
        .Where(account => account.AccountId == accountId)
        .ToList()
        .FirstOrDefault((InvestmentAccountEntity?) null);

    public IEnumerable<InvestmentAccountEntity> GetAll() => _dbContext.InvestmentAccounts.ToList();

    public IEnumerable<InvestmentAccountEntity> FindByClientId(Guid clientId) => _dbContext.InvestmentAccounts
        .Where(account => account.ClientId == clientId)
        .ToList();
}
