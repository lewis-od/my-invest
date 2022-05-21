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

    [Required]
    [Column("address_line_1")]
    public string AddressLine1 { get; set; } = null!;

    [Required]
    [Column("address_line_2")]
    public string AddressLine2 { get; set; } = null!;

    [Required]
    [Column("address_postcode")]
    public string AddressPostcode { get; set; } = null!;

    [Required]
    [Column("address_is_verified")]
    public bool AddressIsVerified { get; set; } = false;
}
