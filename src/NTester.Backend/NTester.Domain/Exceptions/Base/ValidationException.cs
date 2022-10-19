using System.Net;

namespace NTester.Domain.Exceptions.Base;

/// <summary>
/// Base exception for validation.
/// </summary>
public abstract class ValidationException : RestException
{
    /// <summary>
    /// Creates an instance of the validation exception.
    /// </summary>
    /// <param name="message">Exception message</param>
    protected ValidationException(string message) : base(message)
    {
    }

    /// <inheritdoc cref="RestException.StatusCode"/>
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    /// <inheritdoc cref="RestException.Description"/>
    public override string Description => "A validation error occurred while passing invalid data.";
}