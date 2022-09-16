using FluentValidation;

namespace NTester.Domain.Features.Account.GetUser;

/// <summary>
/// Validator for <see cref="GetUserCommand"/>.
/// </summary>
public class GetUserCommandValidator : AbstractValidator<GetUserCommand>
{
    /// <summary>
    /// Creates an instance of the <see cref="GetUserCommandValidator"/>.
    /// </summary>
    public GetUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}