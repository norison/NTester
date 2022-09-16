namespace NTester.Domain.Exceptions.Auth;

/// <summary>
/// Error codes for the authentication.
/// </summary>
public enum AuthCode
{
    /// <summary>
    /// Error code when the user already exists.
    /// </summary>
    UserAlreadyExists = 2000,
    
    /// <summary>
    /// Error code when incorrect user name or password.
    /// </summary>
    IncorrectUserNameOrPassword = 2001,
    
    /// <summary>
    /// Error code when the access token is invalid.
    /// </summary>
    InvalidAccessToken = 2002,
    
    /// <summary>
    /// Error code when the refresh token is invalid.
    /// </summary>
    InvalidRefreshToken = 2003,
    
    /// <summary>
    /// Error code when the refresh token was not provided.
    /// </summary>
    RefreshTokenWasNotProvided = 2004,
    
    /// <summary>
    /// Error code when the client unsupported.
    /// </summary>
    UnsupportedClient = 2005
}