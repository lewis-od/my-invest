using AutoMapper;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;

namespace MyInvest.Persistence.Clients;

public interface IClientEntityMapper
{
    ClientEntity MapToEntity(Client client);
    Client MapFromEntity(ClientEntity entity, IEnumerable<InvestmentAccount> accounts);
}

public class ClientEntityMapper : IClientEntityMapper
{
    private readonly IMapper _mapper;

    public ClientEntityMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public ClientEntity MapToEntity(Client client) => _mapper.Map<ClientEntity>(client);

    public Client MapFromEntity(ClientEntity entity, IEnumerable<InvestmentAccount> accounts)
    {
        var address = new PostalAddress(entity.AddressLine1, entity.AddressLine2, entity.AddressPostcode);
        if (entity.AddressIsVerified)
        {
            address = address.Verified();
        }
        return new Client(ClientId.From(entity.ClientId), entity.Username, address, accounts);
    }
}
