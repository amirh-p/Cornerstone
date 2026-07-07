using Cornerstone.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Cornerstone.Catalog.Api.Middleware;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails();

        switch (exception)
        {
            case ValidationException validationEx:
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Validation failed";
                problemDetails.Extensions["errors"] = validationEx.Errors.Select(e => e.ErrorMessage);
                break;
            case NotFoundException:
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Title = exception.Message;
                break;
            case InvalidOperationException:
                problemDetails.Status = StatusCodes.Status409Conflict;
                problemDetails.Title = exception.Message;
                break;
            default:
                logger.LogError(exception, "Unhandled exception");
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "An unexpected error occurred.";
                break;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}