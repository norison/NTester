using FluentValidation;
using NTester.Domain.Extensions.Validation;

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
        RuleFor(x => x.Email).Email();
        RuleFor(x => x.UserName).UserName();
        RuleFor(x => x.Password).Password();
        RuleFor(x => x.Name).Name();
        RuleFor(x => x.Surname).Surname();
        RuleFor(x => x.ClientName).ClientName();
    }
}