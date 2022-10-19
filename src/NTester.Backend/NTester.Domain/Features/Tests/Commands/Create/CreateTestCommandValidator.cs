using FluentValidation;
using NTester.Domain.Extensions.Validation;
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
        RuleFor(x => x.UserId).UserId();
        RuleFor(x => x.Title).Title();
        RuleFor(x => x.Description).Description();

        RuleFor(x => x.Questions).NotEmpty();
        RuleForEach(x => x.Questions).SetValidator(new CreateTestQuestionModelValidator());
    }
}