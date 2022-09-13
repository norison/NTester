namespace NTester.Domain.Services.Cookie;

/// <summary>
/// Provides the operations with cookies.
/// </summary>
public interface ICookieService
{
    /// <summary>
    /// Sets the refresh token cookie.
    /// </summary>
    /// <param name="refreshToken">Token to refresh a pair of the access and refresh tokens.</param>
    void SetRefreshToken(string refreshToken);

    /// <summary>
    /// Removes the refresh token cookie.
    /// </summary>
    void RemoveRefreshToken();

    /// <summary>
    /// Gets the refresh token from cookie.
    /// </summary>
    /// <returns>Refresh token.</returns>
    string? GetRefreshToken();
}