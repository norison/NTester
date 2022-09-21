using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions.Auth;

/// <summary>
/// Exception for the case when refresh token is expired.
/// </summary>
public class RefreshTokenExpiredException : ValidationExceptionBase
{
    private const string ErrorMessage = "Refresh token has expired.";

    /// <summary>
    /// Creates an instance of the <see cref="RefreshTokenExpiredException"/>.
    /// </summary>
    public RefreshTokenExpiredException() : base(ErrorMessage)
    {
    }

    /// <inheritdoc cref="RestExceptionBase.Code"/>
    public override int Code => (int)AuthCode.RefreshTokenExpired;
}