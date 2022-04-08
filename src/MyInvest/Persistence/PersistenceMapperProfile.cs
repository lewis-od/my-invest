using AutoMapper;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;
using MyInvest.Persistence.Accounts;
using MyInvest.Persistence.Clients;

namespace MyInvest.Persistence;

public class PersistenceMapperProfile : Profile
{
    public PersistenceMapperProfile()
    {
        CreateMap<Client, ClientEntity>()
            .ForMember(entity => entity.ClientId, opt => opt.MapFrom(client => client.ClientId.Value));
        CreateMap<InvestmentAccount, InvestmentAccountEntity>()
            .ForMember(entity => entity.AccountId, opt => opt.MapFrom(account => account.AccountId.Value));
    }
}
