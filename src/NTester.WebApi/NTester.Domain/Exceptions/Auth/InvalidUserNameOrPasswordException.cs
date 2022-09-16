using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions.Auth;

/// <summary>
/// Exception for the case when invalid user name or password.
/// </summary>
public class InvalidUserNameOrPasswordException : ValidationExceptionBase
{
    private const string ErrorMessage = "Provided invalid user name or password";

    /// <summary>
    /// Creates an instance of the InvalidUserNameOrPassword exception.
    /// </summary>
    public InvalidUserNameOrPasswordException() : base(ErrorMessage)
    {
    }

    /// <inheritdoc cref="RestExceptionBase.Code"/>
    public override int Code => (int)AuthCodes.IncorrectUserNameOrPassword;
}