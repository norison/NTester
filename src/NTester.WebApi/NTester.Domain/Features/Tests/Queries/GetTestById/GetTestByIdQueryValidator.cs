using FluentValidation;
using NTester.Domain.Extensions.Validation;

namespace NTester.Domain.Features.Tests.Queries.GetTestById;

/// <summary>
/// Validator for <see cref="GetTestByIdQuery"/>.
/// </summary>
public class GetTestByIdQueryValidator: AbstractValidator<GetTestByIdQuery>
{
    /// <summary>
    /// Creates an instance of the <see cref="GetTestByIdQueryValidator"/>.
    /// </summary>
    public GetTestByIdQueryValidator()
    {
        RuleFor(x => x.UserId).UserId();
        RuleFor(x => x.Id).NotEmpty();
    }
}