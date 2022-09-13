using FluentValidation;

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
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Surname).NotEmpty().MaximumLength(10);
        RuleFor(x => x.ClientId).NotEmpty();
    }
}