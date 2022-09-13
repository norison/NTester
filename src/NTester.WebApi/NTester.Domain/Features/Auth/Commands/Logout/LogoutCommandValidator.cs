using FluentValidation;

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
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}