namespace MyInvest.Domain.Accounts;

public class AccountNotOpenException : Exception
{
    public AccountNotOpenException()
    {
    }

    public AccountNotOpenException(string? message) : base(message)
    {
    }

    public AccountNotOpenException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
