using FluentValidation;
using NTester.Domain.Extensions.Validation;

namespace NTester.Domain.Features.Auth.Commands.Logout;

/// <summary>
/// Validator of the logout command.
/// </summary>
public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    /// <summary>
    /// Creates an instance of the logout command validator.
    /// </summary>
    public LogoutCommandValidator()
    {
        RuleFor(x => x.UserId).UserId();
        RuleFor(x => x.ClientName).ClientName();
    }
}