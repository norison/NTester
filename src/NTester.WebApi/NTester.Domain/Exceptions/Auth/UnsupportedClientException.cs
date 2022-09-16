using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions.Auth;

/// <summary>
/// Exception for the case when user already exists.
/// </summary>
public class UnsupportedClientException : ValidationExceptionBase
{
    private const string ErrorMessage = "Provided client is not supported - Client ID: '{0}'.";

    /// <summary>
    /// Creates an instance of the model validation exception.
    /// </summary>
    /// <param name="clientId">Id of the client.</param>
    public UnsupportedClientException(Guid clientId) : base(string.Format(ErrorMessage, clientId))
    {
    }

    /// <inheritdoc cref="RestExceptionBase.Code"/>
    public override int Code => (int)AuthCode.UnsupportedClient;
}