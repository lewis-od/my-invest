using MyInvest.Domain.Id;

namespace MyInvest.Domain.Account;

public class SavingsAccount : InvestmentAccount
{
    public decimal SavingsContributions { get; private set; }
    public decimal SavingsAllowance { get; }

    public SavingsAccount(
        AccountId accountId,
        Guid clientId,
        AccountType accountType,
        decimal balance,
        decimal savingsAllowance
    ) : base(accountId, clientId, accountType, balance)
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
        decimal balance,
        decimal savingsAllowance,
        decimal savingsContributions
    ) : this(accountId, clientId, accountType, balance, savingsAllowance)
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