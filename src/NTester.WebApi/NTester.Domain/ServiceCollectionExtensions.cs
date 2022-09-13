using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NTester.Domain.Behaviors;
using NTester.Domain.Services.Auth;
using NTester.Domain.Services.Cookie;
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

        services.AddMediatR(assembly);
        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.Configure<JwtSettings>(configuration.GetSection("Auth:JwtSettings"));
        services.Configure<RefreshTokenSettings>(configuration.GetSection("Auth:RefreshTokenSettings"));

        services.AddScoped<IUserManager, UserManagerWrapper>();
        services.AddScoped<ISignInManager, SignInManagerWrapper>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICookieService, CookieService>();

        services.AddSingleton<ITokenService, TokenService>();

        return services;
    }
}