using CreditScoringEngine.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CreditScoringEngine.API.Middlewares;


public class PropostaNotFoundExceptionsHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not PropostaNotFoundExceptions)
        {
            return false;
        }
        ProblemDetails problemDetails = new ProblemDetails
        {
            Title = "Houve um problema na criação da Proposta!",
            Status = StatusCodes.Status404NotFound,
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails);

        Log.Error($"Houve um problema na criação da Proposta!\n{exception.Message}");
        return true;
    }
}