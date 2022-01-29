using System.ComponentModel.DataAnnotations;
using MyInvest.Domain.Account;

namespace MyInvest.REST.Account;

public class AccountDto
{
    [Required]
    public Guid AccountId { get; set; }
    [Required]
    public string AccountType { get; set; } = "";
    [Required]
    public string Status { get; set; } = "";
    [Required]
    public decimal Balance { get; set; } = 0.0m;
    public SavingsDto? Savings { get; set; }
}