using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions.Account;

/// <summary>
/// Exception for the cases when the user not found.
/// </summary>
public class UserNotFoundException : NotFoundExceptionBase
{
    private const string ErrorMessage = "User not found - User ID: '{0}'.";

    /// <summary>
    /// Creates an instance of the <see cref="UserNotFoundException"/>.
    /// </summary>
    /// <param name="userId">Id of the user.</param>
    public UserNotFoundException(Guid userId) : base(string.Format(ErrorMessage, userId))
    {
    }

    /// <inheritdoc cref="RestExceptionBase.Code"/>
    public override int Code => (int)AccountCode.UserNotFound;
}