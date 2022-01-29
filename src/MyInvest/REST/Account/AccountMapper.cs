using AutoMapper;
using MyInvest.Domain.Account;

namespace MyInvest.REST.Account;

public class AccountMapper
{
    private readonly IMapper _mapper;

    public AccountMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public AccountDto MapToDto(InvestmentAccount investmentAccount) => _mapper.Map<AccountDto>(investmentAccount);
}