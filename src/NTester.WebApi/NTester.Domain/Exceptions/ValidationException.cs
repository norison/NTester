using System.Net;
using NTester.Domain.Exceptions.Base;
using NTester.Domain.Exceptions.Codes;

namespace NTester.Domain.Exceptions;

/// <summary>
/// Exception for validation.
/// </summary>
public class ValidationException : RestExceptionBase
{
    /// <summary>
    /// Creates an instance of the validation exception.
    /// </summary>
    /// <param name="code">Exception code.</param>
    /// <param name="message">Exception message.</param>
    public ValidationException(int code, string message) : base(HttpStatusCode.BadRequest, code, message)
    {
    }

    /// <inheritdoc cref="RestExceptionBase.CodeDescription"/>
    public override string CodeDescription => "Request contains invalid data.";
}