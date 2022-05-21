namespace MyInvest.Domain.Accounts;

public class AccountDoesNotExistException : Exception
{
    public AccountDoesNotExistException()
    {
    }

    public AccountDoesNotExistException(string? message) : base(message)
    {
    }

    public AccountDoesNotExistException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
