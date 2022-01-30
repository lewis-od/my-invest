using System.ComponentModel.DataAnnotations;
using MyInvest.REST.Accounts;

namespace MyInvest.REST.Clients;

public class ClientDto
{
    [Required]
    public Guid ClientId { get; set; }
    
    [Required]
    public string Username { get; set; } = "";
    
    [Required]
    public IEnumerable<AccountDto> InvestmentAccounts { get; set; } = Enumerable.Empty<AccountDto>();
}