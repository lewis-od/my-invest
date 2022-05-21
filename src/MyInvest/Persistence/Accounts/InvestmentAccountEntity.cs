using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyInvest.Persistence.Accounts;

[Table("investment_accounts")]
[Index(nameof(ClientId))]
[Index(nameof(ClientId), nameof(AccountType), IsUnique = true)]
public class InvestmentAccountEntity
{
    [Key]
    [Column("account_id")]
    public Guid AccountId { get; set; }
    
    [Required]
    [Column("client_id")]
    public Guid ClientId { get; set; }

    [Required]
    [Column("type")]
    public string AccountType { get; set; } = null!;

    [Required]
    [Column("status")]
    public string AccountStatus { get; set; } = null!;
    
    [Required]
    [Column("balance")]
    public decimal Balance { get; set; }

    protected bool Equals(InvestmentAccountEntity other)
    {
        return AccountId.Equals(other.AccountId) &&
               ClientId.Equals(other.ClientId) &&
               AccountType == other.AccountType &&
               AccountStatus == other.AccountStatus &&
               Balance == other.Balance;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((InvestmentAccountEntity) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(AccountId, ClientId, AccountType, AccountStatus, Balance);
    }
}
