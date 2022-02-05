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
    public string Username { get; set; } = null!;
}
