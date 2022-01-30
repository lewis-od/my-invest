using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyInvest.Persistence.Clients;

[Table("clients")]
public class ClientEntity
{
    [Key]
    [Column("client_id")]
    public Guid ClientId { get; set; }
    
    [Required]
    [Column("username")]
    public string Username { get; set; }
}
