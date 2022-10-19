namespace NTester.DataContracts.Auth;

/// <summary>
/// Response if the user successfully authenticated.
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// Token to access the resource.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Token to get a new pair of access and refresh tokens.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}