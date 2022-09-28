using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions.Auth;

/// <summary>
/// Exception for the case when user already exists.
/// </summary>
public class UnsupportedClientException : ValidationException
{
    private const string ErrorMessage = "Provided client is not supported - Client Name: '{0}'.";

    /// <summary>
    /// Creates an instance of the model validation exception.
    /// </summary>
    /// <param name="clientName">Name of the client.</param>
    public UnsupportedClientException(string clientName) : base(string.Format(ErrorMessage, clientName))
    {
    }

    /// <inheritdoc cref="RestException.Code"/>
    public override int Code => (int)AuthCode.UnsupportedClient;
}