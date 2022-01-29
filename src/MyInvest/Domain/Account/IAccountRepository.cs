using MyInvest.Domain.Id;

namespace MyInvest.Domain.Account;

public interface IAccountRepository
{
    IEnumerable<InvestmentAccount> GetAll();
    InvestmentAccount? GetById(AccountId accountId);
}