using MyInvest.Domain.Account;
using Microsoft.AspNetCore.Mvc;

namespace MyInvest.REST.Account;

[ApiController]
[Route("accounts")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly AccountMapper _accountMapper;

    public AccountController(IAccountRepository accountRepository, AccountMapper accountMapper)
    {
        _accountRepository = accountRepository;
        _accountMapper = accountMapper;
    }

    [HttpGet]
    [Route("")]
    public ActionResult<IEnumerable<AccountDto>> Index() =>
        Ok(_accountRepository.GetAll().Select(account => _accountMapper.MapToDto(account)));
}