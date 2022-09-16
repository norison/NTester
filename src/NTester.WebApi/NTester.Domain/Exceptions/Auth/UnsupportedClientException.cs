using System.Net;
using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions.Auth;

/// <summary>
/// Exception for the case when user already exists.
/// </summary>
public class UnsupportedClientException : RestExceptionBase
{
    private const string ErrorMessage = "Provided client is not supported - Client ID: '{0}'.";

    /// <summary>
    /// Creates an instance of the model validation exception.
    /// </summary>
    /// <param name="clientId">Id of the client.</param>
    public UnsupportedClientException(Guid clientId) : base(string.Format(ErrorMessage, clientId))
    {
    }

    /// <inheritdoc cref="RestExceptionBase.StatusCode"/>
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    /// <inheritdoc cref="RestExceptionBase.Code"/>
    public override int Code => (int)AuthCodes.UnsupportedClient;
}