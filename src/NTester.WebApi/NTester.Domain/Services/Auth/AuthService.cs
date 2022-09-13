using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using NTester.DataAccess.Data.NTesterDbContext;
using NTester.DataAccess.Entities;
using NTester.DataContracts.Auth;
using NTester.Domain.Exceptions;
using NTester.Domain.Services.Token;

namespace NTester.Domain.Services.Auth;

/// <inheritdoc cref="IAuthService"/>
public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly INTesterDbContext _dbContext;
    private readonly ITokenService _tokenService;
    private readonly RefreshTokenSettings _refreshTokenSettings;

    /// <summary>
    /// Creates an instance of the authentication service.
    /// </summary>
    /// <param name="contextAccessor">Accessor of the http context.</param>
    /// <param name="dbContext">Database context of the application.</param>
    /// <param name="tokenService">Service for the token generation.</param>
    /// <param name="refreshTokenSettings">Settings for the refresh token generation.</param>
    public AuthService(
        IHttpContextAccessor contextAccessor,
        INTesterDbContext dbContext,
        ITokenService tokenService,
        IOptions<RefreshTokenSettings> refreshTokenSettings)
    {
        _contextAccessor = contextAccessor;
        _dbContext = dbContext;
        _tokenService = tokenService;
        _refreshTokenSettings = refreshTokenSettings.Value;
    }

    /// <inheritdoc cref="IAuthService.AuthenticateUserAsync"/>
    public async Task<AuthResponse> AuthenticateUserAsync(
        UserEntity user,
        Guid clientId,
        CancellationToken cancellationToken)
    {
        var client = await _dbContext.Clients.FindAsync(new object[] { clientId }, cancellationToken);
        if (client == null)
        {
            throw new RestException(HttpStatusCode.BadRequest, "Unsupported client.");
        }

        var claims = GenerateClaims(user, clientId);

        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshTokenEntity
        {
            Token = refreshToken,
            AccessToken = accessToken,
            Client = client,
            User = user,
            ExpirationDateTime = DateTime.UtcNow.AddMonths(_refreshTokenSettings.LifeMonths)
        };

        await RemoveOldRefreshTokenIfExistsAsync(user.Id, clientId, cancellationToken);

        await _dbContext.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        AddRefreshTokenToCookies(refreshToken);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    private static IEnumerable<Claim> GenerateClaims(UserEntity user, Guid clientId)
    {
        return new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new("client", clientId.ToString())
        };
    }

    private async Task RemoveOldRefreshTokenIfExistsAsync(
        Guid userId,
        Guid clientId,
        CancellationToken cancellationToken)
    {
        var refreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(
            x => x.UserId == userId && x.ClientId == clientId, cancellationToken);

        if (refreshToken != null)
        {
            _dbContext.RefreshTokens.Remove(refreshToken);
        }
    }

    private void AddRefreshTokenToCookies(string refreshToken)
    {
        _contextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
        {
            Secure = true,
            SameSite = SameSiteMode.Strict,
            HttpOnly = true
        });
    }
}