using System.Net;

namespace NTester.Domain.Exceptions.Base;

/// <summary>
/// Base exception for not found cases.
/// </summary>
public abstract class NotFoundExceptionBase : RestExceptionBase
{
    /// <summary>
    /// Creates an instance of the validation exception.
    /// </summary>
    /// <param name="message">Exception message</param>
    protected NotFoundExceptionBase(string message) : base(message)
    {
    }

    /// <inheritdoc cref="RestExceptionBase.StatusCode"/>
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;

    /// <inheritdoc cref="RestExceptionBase.Description"/>
    public override string Description => "The requested data was not found.";
}