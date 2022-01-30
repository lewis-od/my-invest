namespace MyInvest.Domain.Clients;

public class ClientDoesNotExistException : Exception
{
    public ClientDoesNotExistException()
    {
    }

    public ClientDoesNotExistException(string? message) : base(message)
    {
    }

    public ClientDoesNotExistException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
