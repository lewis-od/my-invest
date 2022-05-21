namespace MyInvest.Domain.Clients;

public class PostalAddress
{
    public string Line1 { get; }
    public string? Line2 { get; }
    public string Postcode { get; }
    public bool IsVerified { get; }

    public PostalAddress(string line1, string? line2, string postcode)
    {
        Line1 = line1;
        Line2 = line2;
        Postcode = postcode;
        IsVerified = false;
    }

    private PostalAddress(string line1, string? line2, string postcode, bool isVerified)
    {
        Line1 = line1;
        Line2 = line2;
        Postcode = postcode;
        IsVerified = isVerified;
    }

    public PostalAddress Verified() => new(Line1, Line2, Postcode, true);

    protected bool Equals(PostalAddress other)
    {
        return Line1 == other.Line1 && Line2 == other.Line2 && Postcode == other.Postcode && IsVerified == other.IsVerified;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((PostalAddress) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Line1, Line2, Postcode, IsVerified);
    }
}
