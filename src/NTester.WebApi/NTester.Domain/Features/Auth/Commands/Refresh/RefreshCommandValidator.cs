using FluentValidation;

namespace NTester.Domain.Features.Auth.Commands.Refresh;

/// <summary>
/// Validator of the refresh command.
/// </summary>
public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
    /// <summary>
    /// Creates an instance of the refresh command validator.
    /// </summary>
    public RefreshCommandValidator()
    {
        RuleFor(x => x.AccessToken).NotEmpty();
    }
}