using System.Net;
using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions.Common;

/// <summary>
/// Exception for model validation.
/// </summary>
public class ModelValidationException : RestExceptionBase
{
    /// <summary>
    /// Creates an instance of the model validation exception.
    /// </summary>
    /// <param name="message">Exception message</param>
    public ModelValidationException(string message) : base(message)
    {
    }

    /// <inheritdoc cref="RestExceptionBase.StatusCode"/>
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    
    /// <inheritdoc cref="RestExceptionBase.Code"/>
    public override int Code => (int)CommonCodes.ModelValidationFailed;
}