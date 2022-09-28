using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions.Auth;

/// <summary>
/// Exception for the case when refresh token was not provided.
/// </summary>
public class RefreshTokenWasNotProvidedException : ValidationException
{
    private const string ErrorMessage = "Refresh token was not provided.";

    /// <summary>
    /// Creates an instance of RefreshTokenWasNotProvided exception.
    /// </summary>
    public RefreshTokenWasNotProvidedException() : base(ErrorMessage)
    {
    }

    /// <inheritdoc cref="RestException.Code"/>
    public override int Code => (int)AuthCode.RefreshTokenWasNotProvided;
}