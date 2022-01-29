using System.ComponentModel.DataAnnotations;

namespace MyInvest.REST.Client;

public class SignUpRequestDto
{
    [Required]
    public string Username { get; set; } = "";
}