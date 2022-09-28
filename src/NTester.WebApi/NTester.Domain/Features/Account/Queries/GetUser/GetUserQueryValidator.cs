using FluentValidation;
using NTester.Domain.Extensions.Validation;

namespace NTester.Domain.Features.Account.Queries.GetUser;

/// <summary>
/// Validator for <see cref="GetUserQuery"/>.
/// </summary>
public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    /// <summary>
    /// Creates an instance of the <see cref="GetUserQueryValidator"/>.
    /// </summary>
    public GetUserQueryValidator()
    {
        RuleFor(x => x.UserId).UserId();
    }
}