namespace MyInvest.Domain.Accounts;

public class ClientAlreadyOwnsAccountException : Exception
{
    public ClientAlreadyOwnsAccountException()
    {
    }

    public ClientAlreadyOwnsAccountException(string? message) : base(message)
    {
    }

    public ClientAlreadyOwnsAccountException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
