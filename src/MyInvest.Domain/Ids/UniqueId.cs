namespace MyInvest.Domain.Ids;

public abstract class UniqueId
{
    public Guid Value { get; }

    protected UniqueId(Guid value)
    {
        Value = value;
    }

    protected UniqueId(string value)
    {
        Value = Guid.Parse(value);
    }

    protected bool Equals(UniqueId other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((UniqueId) obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    
    public override string ToString()
    {
        return Value.ToString();
    }

    public static implicit operator Guid(UniqueId id) => id.Value;
}