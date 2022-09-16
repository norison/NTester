using System.Net;

namespace NTester.Domain.Exceptions.Base;

/// <summary>
/// Base exception for validation.
/// </summary>
public abstract class ValidationExceptionBase : RestExceptionBase
{
    /// <summary>
    /// Creates an instance of the validation exception.
    /// </summary>
    /// <param name="message">Exception message</param>
    protected ValidationExceptionBase(string message) : base(message)
    {
    }

    /// <inheritdoc cref="RestExceptionBase.StatusCode"/>
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    /// <inheritdoc cref="RestExceptionBase.Description"/>
    public override string Description => "A validation error occurred while passing invalid data.";
}