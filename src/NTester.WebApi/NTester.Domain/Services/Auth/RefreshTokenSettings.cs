namespace NTester.Domain.Services.Auth;

/// <summary>
/// Settings for the refresh token generation.
/// </summary>
public class RefreshTokenSettings
{
    /// <summary>
    /// Lifetime of the token in months.
    /// </summary>
    public int LifeMonths { get; set; }
}