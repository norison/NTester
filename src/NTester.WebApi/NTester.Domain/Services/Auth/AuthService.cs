using System.Net;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NTester.DataAccess.Data.NTesterDbContext;
using NTester.DataAccess.Entities;
using NTester.DataContracts.Auth;
using NTester.Domain.Exceptions;
using NTester.Domain.Extensions;
using NTester.Domain.Services.Token;

namespace NTester.Domain.Services.Auth;

/// <inheritdoc cref="IAuthService"/>
public class AuthService : IAuthService
{
    private readonly INTesterDbContext _dbContext;
    private readonly ITokenService _tokenService;
    private readonly RefreshTokenSettings _refreshTokenSettings;

    /// <summary>
    /// Creates an instance of the authentication service.
    /// </summary>
    /// <param name="dbContext">Database context of the application.</param>
    /// <param name="tokenService">Service for the token generation.</param>
    /// <param name="refreshTokenSettings">Settings for the refresh token generation.</param>
    public AuthService(
        INTesterDbContext dbContext,
        ITokenService tokenService,
        IOptions<RefreshTokenSettings> refreshTokenSettings)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
        _refreshTokenSettings = refreshTokenSettings.Value;
    }

    #region Public Methods

    /// <inheritdoc cref="IAuthService.AuthenticateUserAsync(UserEntity, Guid)"/>
    public async Task<AuthResponse> AuthenticateUserAsync(UserEntity user, Guid clientId)
    {
        await ValidateIfClientExistsAsync(clientId);

        var refreshTokenEntity = await GetRefreshTokenOrDefault(user.Id, clientId);
        if (refreshTokenEntity != null)
        {
            _dbContext.RefreshTokens.Remove(refreshTokenEntity);
        }

        var claims = GenerateClaims(user, clientId);
        return await AuthenticateAsync(user.Id, clientId, claims);
    }

    /// <inheritdoc cref="IAuthService.AuthenticateUserAsync(string, string)"/>
    public async Task<AuthResponse> AuthenticateUserAsync(string accessToken, string refreshToken)
    {
        var principal = _tokenService.GetPrincipalFromExpiredAccessToken(accessToken);

        var userId = principal.GetUserId();
        var clientId = principal.GetClientId();

        var refreshTokenEntity = await GetRefreshTokenOrDefault(userId, clientId);

        if (refreshTokenEntity == null || refreshTokenEntity.Token != refreshToken)
        {
            throw new RestException(HttpStatusCode.BadRequest, "Invalid token.");
        }

        _dbContext.RefreshTokens.Remove(refreshTokenEntity);

        return await AuthenticateAsync(userId, clientId, principal.Claims);
    }

    /// <inheritdoc cref="IAuthService.RevokeRefreshTokenAsync"/>
    public async Task RevokeRefreshTokenAsync(string refreshToken, Guid clientId, Guid userId)
    {
        var refreshTokenEntity =
            await _dbContext.RefreshTokens.FindAsync(refreshToken);

        if (refreshTokenEntity == null)
        {
            throw new RestException(HttpStatusCode.NotFound, "Refresh token was not found.");
        }

        if (refreshTokenEntity.ClientId != clientId || refreshTokenEntity.UserId != userId)
        {
            throw new RestException(HttpStatusCode.BadRequest, "Invalid refresh token.");
        }

        _dbContext.RefreshTokens.Remove(refreshTokenEntity);
        await _dbContext.SaveChangesAsync();
    }

    #endregion

    #region Private methods

    private async Task<AuthResponse> AuthenticateAsync(Guid userId, Guid clientId, IEnumerable<Claim> claims)
    {
        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshTokenEntity
        {
            Token = refreshToken,
            UserId = userId,
            ClientId = clientId,
            ExpirationDateTime = DateTime.UtcNow.AddMonths(_refreshTokenSettings.LifeMonths)
        };

        await _dbContext.RefreshTokens.AddAsync(refreshTokenEntity);
        await _dbContext.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    private async Task ValidateIfClientExistsAsync(Guid clientId)
    {
        var client = await _dbContext.Clients.FindAsync(clientId);
        if (client == null)
        {
            throw new RestException(HttpStatusCode.BadRequest, "Unsupported client.");
        }
    }

    private static IEnumerable<Claim> GenerateClaims(UserEntity user, Guid clientId)
    {
        return new List<Claim>
        {
            new("id", user.Id.ToString()),
            new("client", clientId.ToString())
        };
    }

    private async Task<RefreshTokenEntity?> GetRefreshTokenOrDefault(Guid userId, Guid clientId)
    {
        return await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId && x.ClientId == clientId);
    }

    #endregion
}