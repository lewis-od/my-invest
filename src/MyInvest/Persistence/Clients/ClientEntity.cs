using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyInvest.Persistence.Clients;

[Table("clients")]
[Index(nameof(Username), IsUnique = true)]
public class ClientEntity
{
    [Key]
    [Column("client_id")]
    public Guid ClientId { get; set; }
    
    [Required]
    [Column("username")]
    public string Username { get; set; }

    protected bool Equals(ClientEntity other)
    {
        return ClientId.Equals(other.ClientId) && Username == other.Username;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ClientEntity) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ClientId, Username);
    }
}
