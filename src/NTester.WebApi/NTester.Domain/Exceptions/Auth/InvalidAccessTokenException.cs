using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions.Auth;

/// <summary>
/// Exception for the case when access token is invalid.
/// </summary>
public class InvalidAccessTokenException : ValidationException
{
    private const string ErrorMessage = "The access token has invalid values or cannot be verified by the secret.";

    /// <summary>
    /// Creates an instance of InvalidAccessToken exception.
    /// </summary>
    public InvalidAccessTokenException() : base(ErrorMessage)
    {
    }

    /// <inheritdoc cref="RestException.Code"/>
    public override int Code => (int)AuthCode.InvalidAccessToken;
}