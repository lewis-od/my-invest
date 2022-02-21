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
}
