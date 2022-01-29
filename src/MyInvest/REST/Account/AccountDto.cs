using System.ComponentModel.DataAnnotations;
using MyInvest.Domain.Account;

namespace MyInvest.REST.Account;

public class AccountDto
{
    [Required]
    public Guid AccountId { get; set; }
    [Required]
    public AccountTypeDto AccountType { get; set; } = AccountTypeDto.GIA;
    [Required]
    public AccountStatusDto Status { get; set; } = AccountStatusDto.PreOpen;
    [Required]
    public decimal Balance { get; set; } = 0.0m;
    public SavingsDto? Savings { get; set; }
}