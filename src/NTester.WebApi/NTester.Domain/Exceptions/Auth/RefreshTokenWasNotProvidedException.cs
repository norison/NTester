using System.Net;
using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions.Auth;

/// <summary>
/// Exception for the case when refresh token was not provided.
/// </summary>
public class RefreshTokenWasNotProvidedException : RestExceptionBase
{
    private const string ErrorMessage = "Refresh token was not provided.";

    /// <summary>
    /// Creates an instance of RefreshTokenWasNotProvided exception.
    /// </summary>
    public RefreshTokenWasNotProvidedException() : base(ErrorMessage)
    {
    }

    /// <inheritdoc cref="RestExceptionBase.StatusCode"/>
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    /// <inheritdoc cref="RestExceptionBase.Code"/>
    public override int Code => (int)AuthCodes.RefreshTokenWasNotProvided;
}