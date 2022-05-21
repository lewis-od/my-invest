using MyInvest.Domain.Ids;

namespace MyInvest.Domain.Clients;

public class ClientId : UniqueId
{
    private ClientId(Guid value) : base(value)
    {
    }

    private ClientId(string value) : base(value)
    {
    }

    public static ClientId From(Guid value) => new(value);

    public static ClientId From(string value) => new(value);
}