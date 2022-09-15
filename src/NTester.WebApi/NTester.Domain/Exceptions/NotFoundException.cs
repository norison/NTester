using System.Net;
using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions;

/// <summary>
/// Not found exception.
/// </summary>
public class NotFoundException : RestExceptionBase
{
    /// <summary>
    /// Creates an instance of the not found exception.
    /// </summary>
    /// <param name="code">Exception code.</param>
    /// <param name="message">Exception message.</param>
    public NotFoundException(int code, string message) : base(HttpStatusCode.NotFound, code, message)
    {
    }

    /// <inheritdoc cref="RestExceptionBase.CodeDescription"/>
    public override string CodeDescription => "Requested data was not found.";
}