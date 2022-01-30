using MyInvest.Domain.Ids;

namespace MyInvest.Domain.Clients;

public class ClientIdGenerator : IUniqueIdGenerator<ClientId>
{
    public ClientId Generate() => ClientId.From(Guid.NewGuid());
}