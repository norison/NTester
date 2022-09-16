using System.Net;
using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions.Common;

/// <summary>
/// Exception for non-general cases.
/// </summary>
public class NonGeneralException : RestExceptionBase
{
    /// <summary>
    /// Creates an instance of the non general exception.
    /// </summary>
    /// <param name="message">Exception message</param>
    public NonGeneralException(string message) : base(message)
    {
    }

    /// <inheritdoc cref="RestExceptionBase.StatusCode"/>
    public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;

    /// <inheritdoc cref="RestExceptionBase.Code"/>
    public override int Code => (int)CommonCode.NonGeneralErrorOccured;

    /// <inheritdoc cref="RestExceptionBase.Description"/>
    public override string Description => "A non-general error has occurred that should not be in the correct workflow.";
}