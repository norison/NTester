using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NTester.Domain.Behaviors;
using NTester.Domain.Services.SignInManager;
using NTester.Domain.Services.Token;
using NTester.Domain.Services.UserManager;

namespace NTester.Domain;

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
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(assembly);
        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddScoped<IUserManager, UserManagerWrapper>();
        services.AddScoped<ISignInManager, SignInManagerWrapper>();

        services.AddSingleton<ITokenService, TokenService>();

        return services;
    }
}