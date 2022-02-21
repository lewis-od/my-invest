namespace MyInvest.Persistence.Accounts;

public interface IInvestmentAccountDao
{
    void CreateAccount(InvestmentAccountEntity account);
    InvestmentAccountEntity? GetById(Guid accountId);
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

    public InvestmentAccountEntity? GetById(Guid accountId) => _dbContext.InvestmentAccounts
        .Where(account => account.AccountId == accountId)
        .ToList()
        .FirstOrDefault((InvestmentAccountEntity?) null);
}
