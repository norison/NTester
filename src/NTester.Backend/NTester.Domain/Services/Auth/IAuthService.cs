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
    /// <param name="userId">Id of the user.</param>
    /// <param name="clientName">Name of the client.</param>
    /// <returns>Authentication response.</returns>
    Task<AuthResponse> AuthenticateUserAsync(Guid userId, string clientName);
    
    /// <summary>
    /// Authenticates a user.
    /// </summary>
    /// <param name="accessToken">Token for accessing resource.</param>
    /// <param name="refreshToken">Token to refresh a pair of tokens.</param>
    /// <returns></returns>
    Task<AuthResponse> AuthenticateUserAsync(string accessToken, string refreshToken);

    /// <summary>
    /// Revokes a refresh token.
    /// </summary>
    /// <param name="refreshToken">Token to refresh.</param>
    /// <param name="userId">Id of the user.</param>
    /// <param name="clientName">Name of the client.</param>
    Task RevokeRefreshTokenAsync(string refreshToken, Guid userId, string clientName);
}