using MyInvest.Domain.Id;

namespace MyInvest.Domain.Account;

public class InvestmentAccount
{
    public AccountId AccountId { get; }
    public Guid ClientId { get; }
    public AccountType AccountType { get; }
    public decimal Balance { get; private set; }

    public InvestmentAccount(AccountId accountId, Guid clientId, AccountType accountType, decimal balance)
    {
        AccountId = accountId;
        ClientId = clientId;
        AccountType = accountType;
        Balance = balance;
    }

    public virtual void CreditBalance(decimal amount)
    {
        Balance += amount;
    }
}