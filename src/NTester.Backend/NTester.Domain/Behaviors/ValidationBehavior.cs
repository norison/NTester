using FluentValidation;
using MediatR;
using NTester.Domain.Exceptions.Common;

namespace NTester.Domain.Behaviors;

/// <summary>
/// Provides validation for commands and queries.
/// </summary>
/// <typeparam name="TRequest">Request type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Creates an instance of the validation behavior.
    /// </summary>
    /// <param name="validators">List of the validators.</param>
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <inheritdoc cref="IPipelineBehavior{TRequest,TResponse}.Handle"/>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResult = _validators
            .Select(x => x.Validate(context))
            .FirstOrDefault(x => !x.IsValid);

        if (validationResult == null)
        {
            return await next();
        }

        var errorMessage = validationResult.Errors.First().ErrorMessage;
        throw new ModelValidationException(errorMessage);
    }
}