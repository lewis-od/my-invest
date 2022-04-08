using AutoMapper;
using MyInvest.Domain.Accounts;

namespace MyInvest.Persistence.Accounts;

public interface IInvestmentAccountEntityMapper
{
    InvestmentAccountEntity MapToEntity(InvestmentAccount account);
    InvestmentAccount MapFromEntity(InvestmentAccountEntity entity);
}

public class InvestmentAccountEntityMapper : IInvestmentAccountEntityMapper
{
    private readonly IMapper _mapper;
    private readonly AccountFactory _accountFactory;

    public InvestmentAccountEntityMapper(IMapper mapper, AccountFactory accountFactory)
    {
        _mapper = mapper;
        _accountFactory = accountFactory;
    }

    public InvestmentAccountEntity MapToEntity(InvestmentAccount account) => _mapper.Map<InvestmentAccountEntity>(account);

    public InvestmentAccount MapFromEntity(InvestmentAccountEntity entity) =>
        _accountFactory.CreateAccount(
            AccountId.From(entity.AccountId),
            entity.ClientId,
            _mapper.Map<AccountType>(entity.AccountType),
            _mapper.Map<AccountStatus>(entity.AccountStatus),
            entity.Balance
        );
}
