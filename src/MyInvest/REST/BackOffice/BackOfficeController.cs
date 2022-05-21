using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using MyInvest.Domain.BackOffice;
using MyInvest.Domain.Clients;

namespace MyInvest.REST.BackOffice;

[ApiController]
[Route("back-office")]
[Produces(MediaTypeNames.Application.Json)]
public class BackOfficeController : ControllerBase
{
    private readonly IBackOfficeService _backOfficeService;

    public BackOfficeController(IBackOfficeService backOfficeService)
    {
        _backOfficeService = backOfficeService;
    }

    [HttpPut]
    [Route("clients/{clientId:guid}/address/verify")]
    public ActionResult VerifyClientAddress(Guid clientId)
    {
        try
        {
            _backOfficeService.VerifyAddress(ClientId.From(clientId));
        }
        catch (ClientDoesNotExistException)
        {
            return new NotFoundResult();
        }

        return new NoContentResult();
    }
}
