namespace MyInvest.Domain.Accounts;

public class SavingsAllowanceExceededException : Exception
{
    public SavingsAllowanceExceededException()
    {
    }

    public SavingsAllowanceExceededException(string? message) : base(message)
    {
    }

    public SavingsAllowanceExceededException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}