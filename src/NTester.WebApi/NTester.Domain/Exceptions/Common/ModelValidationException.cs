using NTester.Domain.Exceptions.Base;

namespace NTester.Domain.Exceptions.Common;

/// <summary>
/// Exception for model validation.
/// </summary>
public class ModelValidationException : ValidationException
{
    /// <summary>
    /// Creates an instance of the model validation exception.
    /// </summary>
    /// <param name="message">Exception message</param>
    public ModelValidationException(string message) : base(message)
    {
    }

    /// <inheritdoc cref="RestException.Code"/>
    public override int Code => (int)CommonCode.ModelValidationFailed;
}