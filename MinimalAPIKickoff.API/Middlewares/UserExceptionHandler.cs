using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MinimalAPIKickoff.Application.Exceptions;
using Serilog;

namespace MinimalAPIKickoff.API.Middlewares;

public class UserExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not UserExceptions)
        {
            return false;
        }
        ProblemDetails problemDetails = new ProblemDetails
        {
            Title = "Houve um problema na criação do Usuario",
            Status = StatusCodes.Status400BadRequest,
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails);

        Log.Error("Houve um problema na criação do Usuario!");
        return true;
    }
}
