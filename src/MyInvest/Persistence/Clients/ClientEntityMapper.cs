using AutoMapper;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;

namespace MyInvest.Persistence.Clients;

public class ClientEntityMapper : IEntityMapper<ClientEntity, Client>
{
    private readonly IMapper _mapper;

    public ClientEntityMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public ClientEntity MapToEntity(Client client) => _mapper.Map<ClientEntity>(client);

    // TODO: Find a way to replace this with AutoMapper without modifying domain model
    public Client MapFromEntity(ClientEntity entity) => new(ClientId.From(entity.ClientId), entity.Username, Enumerable.Empty<InvestmentAccount>());
}
