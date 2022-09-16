using NTester.DataContracts;
using NTester.Domain.Exceptions.Base;
using NTester.Domain.Exceptions.Common;

namespace NTester.WebApi.Middlewares.CustomExceptionHandler;

/// <summary>
/// Middleware to handle all exceptions in the application.
/// </summary>
public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Creates an instance of the custom exception handler.
    /// </summary>
    /// <param name="next">Next middleware action.</param>
    public CustomExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invokes the exception middleware.
    /// </summary>
    /// <param name="context">Context of the request.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var restException = exception as RestExceptionBase ?? new NonGeneralException(exception.Message);

        context.Response.StatusCode = (int)restException.StatusCode;
        context.Response.ContentType = "application/json";

        var errorResponse = new ErrorResponse
        {
            Code = restException.Code,
            Message = restException.Message,
            Description = restException.Description
        };

        await context.Response.WriteAsJsonAsync(errorResponse);
    }
}