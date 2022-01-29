using MyInvest.Domain.Client;
using Microsoft.AspNetCore.Mvc;

namespace MyInvest.REST.Client;

[ApiController]
[Route("clients")]
public class ClientController : ControllerBase
{
    private readonly ClientService _clientService;
    private readonly IClientRepository _clientRepository;
    private readonly ClientMapper _clientMapper;

    public ClientController(ClientService clientService, IClientRepository clientRepository, ClientMapper clientMapper)
    {
        _clientService = clientService;
        _clientRepository = clientRepository;
        _clientMapper = clientMapper;
    }

    [HttpPost]
    [Route("sign-up")]
    public ActionResult<ClientDto> SignUp([FromBody] SignUpRequestDto signUpRequest)
    {
        var newClient = _clientService.SignUp(signUpRequest.Username);
        return _clientMapper.MapToDto(newClient);
    }

    [HttpGet]
    [Route("{clientId:guid}")]
    public ActionResult<ClientDto> GetById(Guid clientId)
    {
        var client = _clientRepository.GetById(ClientId.From(clientId));
        if (client == null)
        {
            return NotFound();
        }

        return _clientMapper.MapToDto(client);
    }
}