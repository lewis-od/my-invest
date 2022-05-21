using MyInvest.Domain.Clients;

namespace MyInvest.Domain.Accounts;

public interface IAccountRepository
{
    IEnumerable<InvestmentAccount> GetAll();
    InvestmentAccount? GetById(AccountId accountId);
    IEnumerable<InvestmentAccount> FindByClientId(ClientId clientId);
    void Create(InvestmentAccount newAccount);
    void Update(InvestmentAccount account);
}