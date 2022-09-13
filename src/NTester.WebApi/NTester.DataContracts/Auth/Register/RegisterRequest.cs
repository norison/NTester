namespace NTester.DataContracts.Auth.Register;

/// <summary>
/// Request for user registration.
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// User name of the user.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Email of the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Password of the user.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Name of the user.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Surname of the user.
    /// </summary>
    public string Surname { get; set; } = string.Empty;
}