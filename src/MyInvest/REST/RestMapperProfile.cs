using AutoMapper;
using MyInvest.Domain.Accounts;
using MyInvest.REST.Accounts;
using MyInvest.REST.Clients;

namespace MyInvest.REST;

public class RestMapperProfile : Profile
{
    public RestMapperProfile()
    {
        CreateMap<InvestmentAccount, AccountDto>()
            .ForMember(dto => dto.Status, member => member.MapFrom(account => account.AccountStatus))
            .ForMember(dto => dto.Savings, member => member.MapFrom<SavingsDto?>(account => null));
        CreateMap<SavingsAccount, AccountDto>()
            .ForMember(dto => dto.Status, member => member.MapFrom(account => account.AccountStatus))
            .ForPath(dto => dto.Savings!.Allowance, member => member.MapFrom(account => account.SavingsAllowance))
            .ForPath(dto => dto.Savings!.Contributions, member => member.MapFrom(account => account.SavingsContributions));
        CreateMap<Domain.Clients.Client, ClientDto>();
    }
}
