namespace MyInvest.Domain.Transactions;

public class Transaction
{
    public TransactionId TransactionId { get; init; } = null!;
    public string MessageAuthenticationCode { get; init; } = null!;
    public decimal Amount { get; init; }

    protected bool Equals(Transaction other)
    {
        return TransactionId.Equals(other.TransactionId) && MessageAuthenticationCode == other.MessageAuthenticationCode && Amount == other.Amount;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Transaction) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(TransactionId, MessageAuthenticationCode, Amount);
    }
}
