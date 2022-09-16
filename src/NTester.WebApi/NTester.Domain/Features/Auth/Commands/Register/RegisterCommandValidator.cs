using FluentValidation;
using NTester.Domain.Extensions;

namespace NTester.Domain.Features.Auth.Commands.Register;

/// <summary>
/// Validator of the registration command.
/// </summary>
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    /// <summary>
    /// Creates an instance of the registration command validator.
    /// </summary>
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.UserName).NotEmpty().MinimumLength(2);
        RuleFor(x => x.Password).Password();
        RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(10).NoWhiteSpaces();
        RuleFor(x => x.Surname).NotEmpty().MinimumLength(2).MaximumLength(10).NoWhiteSpaces();
        RuleFor(x => x.ClientId).NotEmpty();
    }
}