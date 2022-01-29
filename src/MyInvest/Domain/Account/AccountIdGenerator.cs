using MyInvest.Domain.Id;

namespace MyInvest.Domain.Account;

public class AccountIdGenerator : IUniqueIdGenerator<AccountId>
{
    public AccountId Generate() => AccountId.From(Guid.NewGuid());
}