namespace NTester.Domain.Services.Token;

/// <summary>
/// Settings for the access token.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Secret of the token.
    /// </summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    /// Issuer of the token.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Audience of the token.
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Life time of the token.
    /// </summary>
    public TimeSpan LifeTime { get; set; }
}