using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NTester.DataAccess.Data.NTesterDbContext;
using NTester.DataAccess.Entities;
using NTester.DataContracts.Auth;
using NTester.Domain.Constants;
using NTester.Domain.Exceptions.Auth;
using NTester.Domain.Extensions;
using NTester.Domain.Services.DateTime;
using NTester.Domain.Services.Token;

namespace NTester.Domain.Services.Auth;

/// <inheritdoc cref="IAuthService"/>
public class AuthService : IAuthService
{
    private readonly INTesterDbContext _dbContext;
    private readonly ITokenService _tokenService;
    private readonly IDateTimeService _dateTimeService;
    private readonly RefreshTokenSettings _refreshTokenSettings;

    /// <summary>
    /// Creates an instance of the authentication service.
    /// </summary>
    /// <param name="dbContext">Database context of the application.</param>
    /// <param name="tokenService">Service for the token generation.</param>
    /// <param name="dateTimeService">Service for the date and time generation.</param>
    /// <param name="refreshTokenSettings">Settings for the refresh token generation.</param>
    public AuthService(
        INTesterDbContext dbContext,
        ITokenService tokenService,
        IDateTimeService dateTimeService,
        IOptions<RefreshTokenSettings> refreshTokenSettings)
    {
        _dbContext = dbContext;
        _tokenService = tokenService;
        _dateTimeService = dateTimeService;
        _refreshTokenSettings = refreshTokenSettings.Value;
    }

    #region Public Methods

    /// <inheritdoc cref="IAuthService.AuthenticateUserAsync(Guid, Guid)"/>
    public async Task<AuthResponse> AuthenticateUserAsync(Guid userId, Guid clientId)
    {
        await ValidateIfClientExistsAsync(clientId);

        var refreshTokenEntity = await GetRefreshTokenOrDefault(userId, clientId);
        if (refreshTokenEntity != null)
        {
            _dbContext.RefreshTokens.Remove(refreshTokenEntity);
        }

        var claims = GenerateClaims(userId, clientId);
        return await AuthenticateAsync(userId, clientId, claims);
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
            throw new InvalidRefreshTokenException();
        }

        _dbContext.RefreshTokens.Remove(refreshTokenEntity);

        return await AuthenticateAsync(userId, clientId, principal.Claims);
    }

    /// <inheritdoc cref="IAuthService.RevokeRefreshTokenAsync"/>
    public async Task RevokeRefreshTokenAsync(string refreshToken, Guid clientId, Guid userId)
    {
        var refreshTokenEntity = await _dbContext.RefreshTokens.FindAsync(refreshToken);

        if (refreshTokenEntity == null || refreshTokenEntity.ClientId != clientId ||
            refreshTokenEntity.UserId != userId)
        {
            throw new InvalidRefreshTokenException();
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
            ExpirationDateTime = _dateTimeService.UtcNow.AddMonths(_refreshTokenSettings.LifeMonths)
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
        var isClientExists = await _dbContext.Clients.AnyAsync(x => x.Id == clientId);

        if (!isClientExists)
        {
            throw new UnsupportedClientException(clientId);
        }
    }

    private static IEnumerable<Claim> GenerateClaims(Guid userId, Guid clientId)
    {
        return new List<Claim>
        {
            new(ClaimConstants.UserIdClaimTypeName, userId.ToString()),
            new(ClaimConstants.ClientIdClaimTypeName, clientId.ToString())
        };
    }

    private async Task<RefreshTokenEntity?> GetRefreshTokenOrDefault(Guid userId, Guid clientId)
    {
        return await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId && x.ClientId == clientId);
    }

    #endregion
}