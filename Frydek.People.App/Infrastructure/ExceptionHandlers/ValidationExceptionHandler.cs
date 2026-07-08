using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Frydek.People.App.Infrastructure.ExceptionHandlers;

public class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(
            validationException.Errors,
            cancellationToken);

        return true;
    }
}
