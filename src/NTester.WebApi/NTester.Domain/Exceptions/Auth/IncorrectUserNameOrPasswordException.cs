using System.Net;
using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions.Auth;

/// <summary>
/// Exception for the case when incorrect user name or password.
/// </summary>
public class IncorrectUserNameOrPasswordException : ValidationExceptionBase
{
    private const string ErrorMessage = "Provided incorrect user name or password";

    /// <summary>
    /// Creates an instance of the IncorrectUserNameOrPassword exception.
    /// </summary>
    public IncorrectUserNameOrPasswordException() : base(ErrorMessage)
    {
    }

    /// <inheritdoc cref="RestExceptionBase.Code"/>
    public override int Code => (int)AuthCodes.IncorrectUserNameOrPassword;
}