using AutoMapper;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;
using MyInvest.Persistence.Clients;

namespace MyInvest.Persistence;

public class PersistenceMapperModule : IMapperModule
{
    public void Configure(IMapperConfigurationExpression config)
    {
        config.CreateMap<Client, ClientEntity>()
            .ForMember(entity => entity.ClientId, opt => opt.MapFrom(client => client.ClientId.Value));
    }
}
