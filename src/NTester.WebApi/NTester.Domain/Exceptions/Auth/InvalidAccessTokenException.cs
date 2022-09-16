using System.Net;
using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions.Auth;

/// <summary>
/// Exception for the case when access token is invalid.
/// </summary>
public class InvalidAccessTokenException : RestExceptionBase
{
    private const string ErrorMessage = "The access token has invalid values or cannot be verified by the secret.";

    /// <summary>
    /// Creates an instance of InvalidAccessToken exception.
    /// </summary>
    public InvalidAccessTokenException() : base(ErrorMessage)
    {
    }

    /// <inheritdoc cref="RestExceptionBase.StatusCode"/>
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    /// <inheritdoc cref="RestExceptionBase.Code"/>
    public override int Code => (int)AuthCodes.InvalidAccessToken;
}