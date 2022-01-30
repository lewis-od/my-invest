using AutoMapper;
using MyInvest.Domain.Accounts;

namespace MyInvest.REST.Accounts;

public class AccountMapper
{
    private readonly IMapper _mapper;

    public AccountMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public AccountDto MapToDto(InvestmentAccount investmentAccount) => _mapper.Map<AccountDto>(investmentAccount);
}