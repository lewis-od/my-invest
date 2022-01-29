using MyInvest.Domain.Id;

namespace MyInvest.Domain.Client;

public class ClientIdGenerator : IUniqueIdGenerator<ClientId>
{
    public ClientId Generate() => ClientId.From(Guid.NewGuid());
}