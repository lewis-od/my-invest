namespace MyInvest.Domain.Accounts;

public class InvestmentAccount
{
    public AccountId AccountId { get; }
    public Guid ClientId { get; }
    public AccountType AccountType { get; }
    public AccountStatus AccountStatus { get; private set; }
    public decimal Balance { get; private set; }

    public InvestmentAccount(
        AccountId accountId,
        Guid clientId,
        AccountType accountType,
        AccountStatus accountStatus,
        decimal balance)
    {
        AccountId = accountId;
        ClientId = clientId;
        AccountType = accountType;
        AccountStatus = accountStatus;
        Balance = balance;
    }

    public virtual void CreditBalance(decimal amount)
    {
        Balance += amount;
    }

    public void OpenAccount()
    {
        AccountStatus = AccountStatus.Open;
    }
}