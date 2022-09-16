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
    /// <param name="message">Exception message.</param>
    protected RestExceptionBase(string message) : base(message)
    {
    }

    /// <summary>
    /// HTTP status code.
    /// </summary>
    public abstract HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Code of the exception.
    /// </summary>
    public abstract int Code { get; }
}