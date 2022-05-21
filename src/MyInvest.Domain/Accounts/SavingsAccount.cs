namespace MyInvest.Domain.Accounts;

public class SavingsAccount : InvestmentAccount
{
    public decimal SavingsContributions { get; private set; }
    public decimal SavingsAllowance { get; }

    public SavingsAccount(
        AccountId accountId,
        Guid clientId,
        AccountType accountType,
        AccountStatus accountStatus,
        decimal balance,
        decimal savingsAllowance
    ) : base(accountId, clientId, accountType, accountStatus, balance)
    {
        if (accountType != AccountType.ISA && accountType != AccountType.JISA)
        {
            throw new ArgumentException("Savings account must have type ISA or JISA");
        }

        SavingsAllowance = savingsAllowance;
        SavingsContributions = 0.0m;
    }

    public SavingsAccount(
        AccountId accountId,
        Guid clientId,
        AccountType accountType,
        AccountStatus accountStatus,
        decimal balance,
        decimal savingsAllowance,
        decimal savingsContributions
    ) : this(accountId, clientId, accountType, accountStatus, balance, savingsAllowance)
    {
        SavingsContributions = savingsContributions;
    }

    public override void CreditBalance(decimal amount)
    {
        var newContributions = SavingsContributions + amount;
        if (newContributions > SavingsAllowance)
        {
            throw new SavingsAllowanceExceededException($"Adding ${amount} would take contributions to " +
                                                        $"${newContributions}, exceeding the allowance of " +
                                                        SavingsAllowance);
        }

        base.CreditBalance(amount);
        SavingsContributions = newContributions;
    }
}