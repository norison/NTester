using System.Net;

namespace NTester.Domain.Exceptions.Base;

/// <summary>
/// Base exception of the application.
/// </summary>
public abstract class RestException : Exception
{
    /// <summary>
    /// Creates an instance of the base exception of the application.
    /// </summary>
    /// <param name="message">Exception message.</param>
    protected RestException(string message) : base(message)
    {
    }

    /// <summary>
    /// HTTP status code.
    /// </summary>
    public abstract HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Code of the error.
    /// </summary>
    public abstract int Code { get; }

    /// <summary>
    /// Description of the exception.
    /// </summary>
    public abstract string Description { get; }
}