using AutoMapper;
using MyInvest.Domain.Accounts;

namespace MyInvest.REST.Accounts;

public class AccountDtoMapper
{
    private readonly IMapper _mapper;

    public AccountDtoMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public AccountDto MapToDto(InvestmentAccount investmentAccount) => _mapper.Map<AccountDto>(investmentAccount);
}