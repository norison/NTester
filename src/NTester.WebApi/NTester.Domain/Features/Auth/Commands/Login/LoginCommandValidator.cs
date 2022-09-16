using FluentValidation;
using NTester.Domain.Extensions;

namespace NTester.Domain.Features.Auth.Commands.Login;

/// <summary>
/// Validator of the login command.
/// </summary>
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    /// <summary>
    /// Creates an instance of the login command validator.
    /// </summary>
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Password).Password();
        RuleFor(x => x.ClientId).NotEmpty();
    }
}