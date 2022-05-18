using System.ComponentModel.DataAnnotations;

namespace MyInvest.REST.Clients;

public class PostalAddressDto
{
    [Required]
    public string Line1 { get; init; } = null!;

    [Required]
    public string Line2 { get; init; } = null!;

    [Required]
    public string Postcode { get; init; } = null!;
}
