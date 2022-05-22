using System.ComponentModel.DataAnnotations;

namespace MyInvest.REST.Clients;

public class PostalAddressDto
{
    [Required]
    public string Line1 { get; set; } = string.Empty;

    [Required]
    public string Line2 { get; set; } = string.Empty;

    [Required]
    public string Postcode { get; set; } = string.Empty;
}
