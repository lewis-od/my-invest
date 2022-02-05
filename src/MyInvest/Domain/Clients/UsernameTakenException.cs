namespace MyInvest.Domain.Clients;

public class UsernameTakenException : Exception
{
    public UsernameTakenException()
    {
    }

    public UsernameTakenException(string? message) : base(message)
    {
    }

    public UsernameTakenException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
