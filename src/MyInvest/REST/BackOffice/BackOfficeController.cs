using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using MyInvest.Domain.BackOffice;
using MyInvest.Domain.Clients;

namespace MyInvest.REST.BackOffice;

[ApiController]
[Route("back-office")]
[Produces(MediaTypeNames.Application.Json)]
public class BackOfficeController : ControllerBase
{
    private readonly BackOfficeService _backOfficeService;

    public BackOfficeController(BackOfficeService backOfficeService)
    {
        _backOfficeService = backOfficeService;
    }

    [HttpPut]
    [Route("clients/{clientId:guid}/address/verify")]
    public void VerifyClientAddress(Guid clientId)
    {
        _backOfficeService.VerifyAddress(ClientId.From(clientId));
    }
}
