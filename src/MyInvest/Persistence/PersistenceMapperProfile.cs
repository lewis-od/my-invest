using AutoMapper;
using MyInvest.Domain.Clients;
using MyInvest.Persistence.Clients;

namespace MyInvest.Persistence;

public class PersistenceMapperProfile : Profile
{
    public PersistenceMapperProfile()
    {
        CreateMap<Client, ClientEntity>()
            .ForMember(entity => entity.ClientId, opt => opt.MapFrom(client => client.ClientId.Value));
    }
}
