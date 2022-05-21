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
            .ForMember(entity => entity.ClientId, opt => opt.MapFrom(client => client.ClientId.Value))
            .ForMember(entity => entity.AddressLine1, opt => opt.MapFrom(client => client.Address.Line1))
            .ForMember(entity => entity.AddressLine2, opt => opt.MapFrom(client => client.Address.Line2))
            .ForMember(entity => entity.AddressPostcode, opt => opt.MapFrom(client => client.Address.Postcode))
            .ForMember(entity => entity.AddressIsVerified, opt => opt.MapFrom(client => client.Address.IsVerified));
        CreateMap<InvestmentAccount, InvestmentAccountEntity>()
            .ForMember(entity => entity.AccountId, opt => opt.MapFrom(account => account.AccountId.Value));
    }
}
