using System.ComponentModel.DataAnnotations;

namespace MyInvest.REST.Clients;

public class SignUpRequestDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public PostalAddressDto Address { get; set; } = new();
}