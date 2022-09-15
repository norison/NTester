using System.Net;

namespace NTester.Domain.Exceptions.Base;

/// <summary>
/// Base exception of the application.
/// </summary>
public abstract class RestExceptionBase : Exception
{
    /// <summary>
    /// Creates an instance of the base exception of the application.
    /// </summary>
    /// <param name="statusCode">HTTP status code.</param>
    /// <param name="code">Exception code.</param>
    /// <param name="message">Exception message.</param>
    protected RestExceptionBase(HttpStatusCode statusCode, int code, string message) : base(message)
    {
        StatusCode = statusCode;
        Code = code;
    }

    /// <summary>
    /// HTTP status code.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Code of the exception.
    /// </summary>
    public int Code { get; }

    /// <summary>
    /// CodeDescription of the exception code.
    /// </summary>
    public abstract string CodeDescription { get; }
}