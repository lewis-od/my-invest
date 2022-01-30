using MyInvest.Domain.Ids;

namespace MyInvest.Domain.Accounts;

public class AccountId : UniqueId
{
    private AccountId(Guid value) : base(value)
    {
    }

    private AccountId(string value) : base(value)
    {
    }

    public static AccountId From(Guid value) => new(value);

    public static AccountId From(string value) => new(value);
}