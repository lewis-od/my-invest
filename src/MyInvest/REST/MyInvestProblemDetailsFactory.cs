using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyInvest.Domain.Account;
using MyInvest.Domain.Client;

namespace MyInvest.REST;

public class MyInvestProblemDetailsFactory : ProblemDetailsFactory
{
    public override ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null
    )
    {
        var context = httpContext.Features.Get<IExceptionHandlerFeature>();
        return context?.Error switch
        {
            ClientDoesNotExistException exception => NewProblemDetails(httpContext, StatusCodes.Status400BadRequest, "Client does not exist", type,
                exception.Message, instance),
            ClientAlreadyOwnsAccountException exception => NewProblemDetails(httpContext, StatusCodes.Status400BadRequest,
                "Client already owns account", type, exception.Message, instance),
            _ => NewProblemDetails(httpContext, statusCode, title, type, detail, instance)
        };
    }

    private static ProblemDetails NewProblemDetails(
        HttpContext httpContext,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null
    )
    {
        var problemDetails = new ProblemDetails()
        {
            Status = statusCode ?? StatusCodes.Status500InternalServerError,
            Title = title,
            Type = type,
            Detail = detail,
            Instance = instance
        };
        var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
        if (traceId != null)
        {
            problemDetails.Extensions["traceId"] = traceId;
        }

        return problemDetails;
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        ModelStateDictionary modelStateDictionary,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null
    )
    {
        var problemDetails = new ValidationProblemDetails(modelStateDictionary)
        {
            Status = statusCode ?? StatusCodes.Status400BadRequest,
            Type = type,
            Detail = detail,
            Instance = instance,
        };

        if (title != null)
        {
            // For validation problem details, don't overwrite the default title with null.
            problemDetails.Title = title;
        }

        return problemDetails;
    }
}
