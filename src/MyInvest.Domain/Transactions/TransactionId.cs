using MyInvest.Domain.Ids;

namespace MyInvest.Domain.Transactions;

public class TransactionId : UniqueId
{
    private TransactionId(Guid value) : base(value)
    {
    }

    private TransactionId(string value) : base(value)
    {
    }
    
    public static TransactionId From(Guid value) => new(value);

    public static TransactionId From(string value) => new(value);
}
