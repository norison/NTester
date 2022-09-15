using System.Net;
using NTester.Domain.Exceptions.Base;
using NTester.Domain.Exceptions.Codes;

namespace NTester.Domain.Exceptions;

/// <summary>
/// Exception for non-general cases.
/// </summary>
public class NonGeneralException : RestExceptionBase
{
    /// <summary>
    /// Creates an instance of the non general exception.
    /// </summary>
    /// <param name="message">Exception message</param>
    public NonGeneralException(string message)
        : base(HttpStatusCode.InternalServerError, (int)CommonCodes.NonGeneralErrorOccured, message)
    {
    }

    /// <inheritdoc cref="RestExceptionBase.CodeDescription"/>
    public override string CodeDescription => "This type of error should not occur in the correct workflow";
}