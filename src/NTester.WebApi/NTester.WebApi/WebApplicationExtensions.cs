using NTester.WebApi.Middlewares.CustomExceptionHandler;

namespace NTester.WebApi;

/// <summary>
/// Extensions of the web application.
/// </summary>
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

        app.UseHttpsRedirection();
        app.UseCustomExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}