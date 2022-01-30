using AutoMapper;
using MyInvest.Domain.Accounts;
using MyInvest.REST.Accounts;
using MyInvest.REST.Clients;

namespace MyInvest.REST;

public class RestMapperModule : IMapperModule
{
    public void Configure(IMapperConfigurationExpression config)
    {
        config.CreateMap<InvestmentAccount, AccountDto>()
            .ForMember(dto => dto.Status, member => member.MapFrom(account => account.AccountStatus));
        config.CreateMap<SavingsAccount, AccountDto>()
            .ForMember(dto => dto.Status, member => member.MapFrom(account => account.AccountStatus))
            .ForPath(dto => dto.Savings!.Allowance, member => member.MapFrom(account => account.SavingsAllowance))
            .ForPath(dto => dto.Savings!.Contributions, member => member.MapFrom(account => account.SavingsContributions));
        config.CreateMap<Domain.Clients.Client, ClientDto>();
    }
}
