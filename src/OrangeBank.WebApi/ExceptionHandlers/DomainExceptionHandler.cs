using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using OrangeBank.Core.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace OrangeBank.WebApi.ExceptionHandlers;

public class DomainExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is DomainException domainException)
        {
            var problemDetails = new
            {
                title = "Domain Error",
                status = (int)HttpStatusCode.BadRequest,
                detail = domainException.Message,
                type = domainException.GetType().Name
            };

            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            httpContext.Response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(problemDetails);
            await httpContext.Response.WriteAsync(json, cancellationToken);

            return true; // <- Interrompe a cadeia aqui!
        }

        return false;
    }
}
