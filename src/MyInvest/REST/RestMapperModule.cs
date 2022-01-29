using AutoMapper;
using MyInvest.Domain;
using MyInvest.Domain.Account;
using MyInvest.REST.Account;
using MyInvest.REST.Client;

namespace MyInvest.REST;

public class RestMapperModule : IMapperModule
{
    public void Configure(IMapperConfigurationExpression config)
    {
        config.CreateMap<InvestmentAccount, AccountDto>();
        config.CreateMap<SavingsAccount, AccountDto>()
            .ForPath(dto => dto.Savings!.Allowance, member => member.MapFrom(account => account.SavingsAllowance))
            .ForPath(dto => dto.Savings!.Contributions, member => member.MapFrom(account => account.SavingsContributions));
        config.CreateMap<Domain.Client.Client, ClientDto>();
    }
}