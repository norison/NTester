namespace NTester.Domain.Exceptions.Auth;

/// <summary>
/// Exception codes for the authentication.
/// </summary>
public enum AuthCodes
{
    /// <summary>
    /// Exception code when the user already exists.
    /// </summary>
    UserAlreadyExists = 2000,
    
    /// <summary>
    /// Exception code when incorrect user name or password.
    /// </summary>
    IncorrectUserNameOrPassword = 2001,
    
    /// <summary>
    /// Exception code when the access token is invalid.
    /// </summary>
    InvalidAccessToken = 2002,
    
    /// <summary>
    /// Exception code when the refresh token is invalid.
    /// </summary>
    InvalidRefreshToken = 2003,
    
    /// <summary>
    /// Exception code when the refresh token was not provided.
    /// </summary>
    RefreshTokenWasNotProvided = 2004,
    
    /// <summary>
    /// Exception code when the client unsupported.
    /// </summary>
    UnsupportedClient = 2005
}