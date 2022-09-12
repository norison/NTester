using NTester.DataAccess;

namespace NTester.WebApi;

/// <summary>
/// Extensions of the service collections.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds required services for the NTester application.
    /// </summary>
    /// <param name="services">Collection of the services.</param>
    /// <param name="configuration">Configuration of the application.</param>
    /// <returns>Collection of the services.</returns>
    public static IServiceCollection AddNTesterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataAccess(configuration);
        services.AddControllers();

        return services;
    }
}