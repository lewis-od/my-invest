using MyInvest.Domain.Ids;

namespace MyInvest.Domain.Accounts;

public class AccountIdGenerator : IUniqueIdGenerator<AccountId>
{
    public AccountId Generate() => AccountId.From(Guid.NewGuid());
}