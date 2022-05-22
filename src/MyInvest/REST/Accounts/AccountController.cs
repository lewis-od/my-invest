using Microsoft.AspNetCore.Mvc;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;
using MyInvest.Domain.Transactions;

namespace MyInvest.REST.Accounts;

[ApiController]
[Route("accounts")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountOpeningService _accountOpeningService;
    private readonly ICashService _cashService;
    private readonly IDtoMapper<InvestmentAccount, AccountDto> _accountDtoMapper;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        IAccountRepository accountRepository,
        IAccountOpeningService accountOpeningService,
        ICashService cashService,
        IDtoMapper<InvestmentAccount, AccountDto> accountDtoMapper,
        ILogger<AccountController> logger
    )
    {
        _accountRepository = accountRepository;
        _accountOpeningService = accountOpeningService;
        _cashService = cashService;
        _accountDtoMapper = accountDtoMapper;
        _logger = logger;
    }

    [HttpGet]
    [Route("{accountId:guid}")]
    public ActionResult<AccountDto> GetAccount(Guid accountId)
    {
        var account = _accountRepository.GetById(AccountId.From(accountId));
        if (account == null)
        {
            _logger.LogInformation("Account {} not found", accountId);
            return new NotFoundResult();
        }

        return _accountDtoMapper.MapToDto(account);
    }

    [HttpPatch]
    [Route("{accountId:guid}/add-cash")]
    public ActionResult AddCash(Guid accountId, [FromBody] AddCashRequestDto request)
    {
        var transaction = new Transaction
        {
            TransactionId = TransactionId.From(request.TransactionId),
            MessageAuthenticationCode = request.MAC,
            Amount = request.Amount,
        };

        try
        {
            _cashService.AddCashToAccount(AccountId.From(accountId), transaction);
        }
        catch (AccountDoesNotExistException)
        {
            _logger.LogError("Account {} not found", accountId);
            return NotFound();
        }
        catch (AccountNotOpenException)
        {
            _logger.LogError("Can't add cash to account {} as it is not in the open state", accountId);
            return Conflict();
        }
        catch (InvalidTransactionException)
        {
            _logger.LogError("Transaction invalid [id={}; mac={}; amount={}]", request.TransactionId, request.MAC, request.Amount);
            return BadRequest();
        }
        
        _logger.LogInformation("Added {} to account {} from transaction {}", request.Amount, accountId, request.TransactionId);
        return NoContent();
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
