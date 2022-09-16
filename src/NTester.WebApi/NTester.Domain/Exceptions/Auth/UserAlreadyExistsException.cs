using System.Net;
using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions.Auth;

/// <summary>
/// Exception for the case when user already exists.
/// </summary>
public class UserAlreadyExistsException : RestExceptionBase
{
    private const string ErrorMessage = "User with the same user name already exists - User name: '{0}'.";

    /// <summary>
    /// Creates an instance of the model validation exception.
    /// </summary>
    /// <param name="userName">User name of the user.</param>
    public UserAlreadyExistsException(string userName) : base(string.Format(ErrorMessage, userName))
    {
    }

    /// <inheritdoc cref="RestExceptionBase.StatusCode"/>
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    /// <inheritdoc cref="RestExceptionBase.Code"/>
    public override int Code => (int)AuthCodes.UserAlreadyExists;
}