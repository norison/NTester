using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NTester.DataAccess.Data.NTesterDbContext;
using NTester.DataAccess.Services.Transaction;

namespace NTester.DataAccess;

/// <summary>
/// Extensions of the service collections.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds data access services to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">Collection of the services.</param>
    /// <param name="configuration">Configuration of the application.</param>
    /// <returns>Collection of the services.</returns>
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<INTesterDbContext, NTesterDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.UseExceptionProcessor();
        });

        services.AddScoped<ITransactionFactory, TransactionFactory>();

        return services;
    }
}