using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions.Common;

/// <summary>
/// Exception for model validation.
/// </summary>
public class ModelValidationException : ValidationExceptionBase
{
    /// <summary>
    /// Creates an instance of the model validation exception.
    /// </summary>
    /// <param name="message">Exception message</param>
    public ModelValidationException(string message) : base(message)
    {
    }

    /// <inheritdoc cref="RestExceptionBase.Code"/>
    public override int Code => (int)CommonCodes.ModelValidationFailed;
}