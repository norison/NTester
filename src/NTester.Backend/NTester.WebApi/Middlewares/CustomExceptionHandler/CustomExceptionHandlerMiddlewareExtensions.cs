using System.Diagnostics.CodeAnalysis;

namespace NTester.WebApi.Middlewares.CustomExceptionHandler;

/// <summary>
/// Extensions for the custom exception handler middleware.
/// </summary>
[ExcludeFromCodeCoverage]
public static class CustomExceptionHandlerMiddlewareExtensions
{
    /// <summary>
    /// Adds the custom exception handler.
    /// </summary>
    /// <param name="app">Application builder.</param>
    /// <returns>Application builder.</returns>
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}