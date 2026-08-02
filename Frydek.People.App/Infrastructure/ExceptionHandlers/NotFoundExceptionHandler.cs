using Frydek.People.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Frydek.People.App.Infrastructure.ExceptionHandlers;

public class NotFoundExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException notFoundException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await httpContext.Response.WriteAsJsonAsync(
            new { message = notFoundException.Message },
            cancellationToken);

        return true;
    }
}
