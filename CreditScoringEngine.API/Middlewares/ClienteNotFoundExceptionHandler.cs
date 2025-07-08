using CreditScoringEngine.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CreditScoringEngine.API.Middlewares;


public class ClienteNotFoundExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ClienteNotFoundException)
        {
            return false;
        }
        ProblemDetails problemDetails = new ProblemDetails
        {
            Title = "Cliente nao encontrado na base de dados",
            Status = StatusCodes.Status404NotFound,
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails);

        Log.Error($"Cliente nao encontrado na base de dados!\n{exception.Message}");
        return true;
    }
}
