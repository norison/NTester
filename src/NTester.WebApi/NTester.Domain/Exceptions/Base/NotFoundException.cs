using System.Net;

namespace NTester.Domain.Exceptions.Base;

/// <summary>
/// Base exception for not found cases.
/// </summary>
public abstract class NotFoundException : RestException
{
    /// <summary>
    /// Creates an instance of the validation exception.
    /// </summary>
    /// <param name="message">Exception message</param>
    protected NotFoundException(string message) : base(message)
    {
    }

    /// <inheritdoc cref="RestException.StatusCode"/>
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;

    /// <inheritdoc cref="RestException.Description"/>
    public override string Description => "The requested data was not found.";
}