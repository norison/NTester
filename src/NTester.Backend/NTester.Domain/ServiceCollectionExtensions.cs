using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NTester.Domain.Behaviors;
using NTester.Domain.Services.Auth;
using NTester.Domain.Services.Cookie;
using NTester.Domain.Services.DateTime;
using NTester.Domain.Services.SignInManager;
using NTester.Domain.Services.Token;
using NTester.Domain.Services.UserManager;

namespace NTester.Domain;

/// <summary>
/// Extensions of the service collections.
/// </summary>
[ExcludeFromCodeCoverage]
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

        services.AddAutoMapper(assembly);

        ConfigureMediator(services, assembly);
        ConfigureValidation(services, assembly);

        services.Configure<JwtSettings>(configuration.GetSection("Auth:JwtSettings"));
        services.Configure<RefreshTokenSettings>(configuration.GetSection("Auth:RefreshTokenSettings"));

        services.AddScoped<IUserManager, UserManagerWrapper>();
        services.AddScoped<ISignInManager, SignInManagerWrapper>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICookieService, CookieService>();

        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddSingleton<ITokenService, TokenService>();

        return services;
    }

    private static void ConfigureMediator(IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    private static void ConfigureValidation(IServiceCollection services, Assembly assembly)
    {
        services.AddValidatorsFromAssembly(assembly);
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
    }
}