using Microsoft.AspNetCore.Mvc;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;

namespace MyInvest.REST.Accounts;

[ApiController]
[Route("accounts")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly AccountOpeningService _accountOpeningService;
    private readonly AccountMapper _accountMapper;

    public AccountController(IAccountRepository accountRepository, AccountOpeningService accountOpeningService, AccountMapper accountMapper)
    {
        _accountRepository = accountRepository;
        _accountOpeningService = accountOpeningService;
        _accountMapper = accountMapper;
    }

    [HttpGet]
    [Route("")]
    public ActionResult<IEnumerable<AccountDto>> Index() =>
        Ok(_accountRepository.GetAll().Select(account => _accountMapper.MapToDto(account)));

    [HttpPost]
    [Route("open/gia")]
    public ActionResult<AccountDto> OpenGia([FromBody] OpenAccountRequestDto openAccountRequest)
    {
        var clientId = ClientId.From(openAccountRequest.ClientId);
        return Ok(OpenAccountOfType(AccountType.GIA, clientId));
    }

    private AccountDto OpenAccountOfType(AccountType accountType, ClientId clientId)
    {
        var account = _accountOpeningService.OpenAccount(clientId, accountType);
        return _accountMapper.MapToDto(account);
    }
}