using NTester.DataAccess.Data.DatabaseInitializer;
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
        InitializeDatabase(app);

        app.UseCustomExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }

    private static void InitializeDatabase(IHost host)
    {
        using var scope = host.Services.CreateScope();

        scope.ServiceProvider
            .GetRequiredService<IDatabaseInitializer>()
            .InitializeAsync()
            .Wait();
    }
}