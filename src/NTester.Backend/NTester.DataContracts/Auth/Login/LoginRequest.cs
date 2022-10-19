namespace NTester.DataContracts.Auth.Login;

/// <summary>
/// Request for user login.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// User name of the user.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Password of the user.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}