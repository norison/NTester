using System.Security.Claims;

namespace NTester.Domain.Services.Token;

/// <summary>
/// Provides functionality for generating a token.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates an access token.
    /// </summary>
    /// <returns>Access token.</returns>
    string GenerateAccessToken(IEnumerable<Claim> claims);

    /// <summary>
    /// Generates a refresh token.
    /// </summary>
    /// <returns>Refresh token.</returns>
    string GenerateRefreshToken();

    /// <summary>
    /// Gets principal from the access token.
    /// </summary>
    /// <param name="accessToken">Access token.</param>
    /// <returns>Principal from the access token.</returns>
    ClaimsPrincipal GetPrincipalFromExpiredAccessToken(string accessToken);
}