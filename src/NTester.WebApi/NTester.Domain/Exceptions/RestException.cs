using System.Net;

namespace NTester.Domain.Exceptions;

/// <summary>
/// Represents a rest exception.
/// </summary>
public class RestException : Exception
{
    /// <summary>
    /// Creates an instance of the rest exception.
    /// </summary>
    /// <param name="statusCode">HTTP status code.</param>
    /// <param name="message">Message of the exception.</param>
    public RestException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    /// <summary>
    /// HTTP status code.
    /// </summary>
    public HttpStatusCode StatusCode { get; }
}