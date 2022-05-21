namespace MyInvest.Domain.Accounts;

public class AccountAlreadyExistsException : Exception
{
    public AccountAlreadyExistsException()
    {
    }

    public AccountAlreadyExistsException(string? message) : base(message)
    {
    }

    public AccountAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
