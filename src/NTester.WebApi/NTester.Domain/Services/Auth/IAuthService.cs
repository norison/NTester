using NTester.DataAccess.Entities;
using NTester.DataContracts.Auth;

namespace NTester.Domain.Services.Auth;

/// <summary>
/// Provides authentication operations.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates a user.
    /// </summary>
    /// <param name="userEntity">Entity of the user.</param>
    /// <param name="clientId">Id of the client.</param>
    /// <returns>Authentication response.</returns>
    Task<AuthResponse> AuthenticateUserAsync(UserEntity userEntity, Guid clientId);

    /// <summary>
    /// Revokes a refresh token.
    /// </summary>
    /// <param name="refreshToken">Token to refresh.</param>
    /// <param name="clientId">Id of the client.</param>
    /// <param name="userId">Id of the user.</param>
    Task RevokeRefreshTokenAsync(string refreshToken, Guid clientId, Guid userId);
}