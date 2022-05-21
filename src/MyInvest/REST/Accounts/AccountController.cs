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
    private readonly AccountDtoMapper _accountDtoMapper;

    public AccountController(IAccountRepository accountRepository, AccountOpeningService accountOpeningService, AccountDtoMapper accountDtoMapper)
    {
        _accountRepository = accountRepository;
        _accountOpeningService = accountOpeningService;
        _accountDtoMapper = accountDtoMapper;
    }

    [HttpGet]
    [Route("{accountId:guid}")]
    public ActionResult<AccountDto> GetAccount(Guid accountId)
    {
        var account = _accountRepository.GetById(AccountId.From(accountId));
        if (account == null)
        {
            return new NotFoundResult();
        }

        return _accountDtoMapper.MapToDto(account);
    }

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
        return _accountDtoMapper.MapToDto(account);
    }
}
