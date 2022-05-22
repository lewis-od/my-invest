using System.ComponentModel.DataAnnotations;

namespace MyInvest.REST.Accounts;

public class AccountDto
{
    [Required]
    public Guid AccountId { get; set; }
    [Required]
    public AccountTypeDto AccountType { get; set; } = AccountTypeDto.GIA;
    [Required]
    public AccountStatusDto Status { get; set; } = AccountStatusDto.PreOpen;
    [Required]
    public decimal Balance { get; set; }
    public SavingsDto? Savings { get; set; }
}