using FluentValidation;
using NTester.Domain.Features.Tests.Commands.Create.Models;

namespace NTester.Domain.Features.Tests.Commands.Create;

/// <summary>
/// Validator for <see cref="CreateTestCommand"/>.
/// </summary>
public class CreateTestCommandValidator : AbstractValidator<CreateTestCommand>
{
    /// <summary>
    /// Creates an instance of the <see cref="CreateTestCommandValidator"/>.
    /// </summary>
    public CreateTestCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).MaximumLength(200);
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.Questions).NotEmpty();
        RuleForEach(x => x.Questions).SetValidator(new CreateTestQuestionModelValidator());
    }
}