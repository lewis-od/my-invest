using System.ComponentModel.DataAnnotations;

namespace MyInvest.REST.Clients;

public class SignUpRequestDto
{
    [Required]
    public string Username { get; set; } = "";

    [Required]
    public PostalAddressDto Address { get; set; } = new();
}