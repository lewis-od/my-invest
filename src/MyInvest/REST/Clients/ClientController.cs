using Microsoft.AspNetCore.Mvc;
using MyInvest.Domain.Clients;

namespace MyInvest.REST.Clients;

[ApiController]
[Route("clients")]
public class ClientController : ControllerBase
{
    private readonly ClientService _clientService;
    private readonly IClientRepository _clientRepository;
    private readonly ClientDtoMapper _clientDtoMapper;

    public ClientController(ClientService clientService, IClientRepository clientRepository, ClientDtoMapper clientDtoMapper)
    {
        _clientService = clientService;
        _clientRepository = clientRepository;
        _clientDtoMapper = clientDtoMapper;
    }

    [HttpPost]
    [Route("sign-up")]
    public ActionResult<ClientDto> SignUp([FromBody] SignUpRequestDto signUpRequest)
    {
        var address = _clientDtoMapper.MapToDomain(signUpRequest.Address);
        var newClient = _clientService.SignUp(signUpRequest.Username, address);
        return _clientDtoMapper.MapToDto(newClient);
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

        return _clientDtoMapper.MapToDto(client);
    }
}