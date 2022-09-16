using System.Diagnostics.CodeAnalysis;
using NTester.WebApi.Middlewares.CustomExceptionHandler;

namespace NTester.WebApi;

/// <summary>
/// Extensions of the web application.
/// </summary>
[ExcludeFromCodeCoverage]
public static class WebApplicationExtensions
{
    /// <summary>
    /// Add required middlewares for the application.
    /// </summary>
    /// <param name="app">Web application.</param>
    /// <returns>Web application.</returns>
    public static WebApplication UseNTester(this WebApplication app)
    {
        if (app.Environment.IsProduction())
        {
            app.UseHsts();
        }

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = string.Empty;
            options.SwaggerEndpoint("swagger/v1/swagger.json", "NTester Web API");
        });
        
        app.UseHttpsRedirection();
        app.UseCustomExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}