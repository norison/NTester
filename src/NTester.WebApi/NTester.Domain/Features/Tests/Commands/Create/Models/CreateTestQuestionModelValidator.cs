using FluentValidation;

namespace NTester.Domain.Features.Tests.Commands.Create.Models;

/// <summary>
/// Validator for <see cref="CreateTestQuestionModel"/>.
/// </summary>
public class CreateTestQuestionModelValidator : AbstractValidator<CreateTestQuestionModel>
{
    /// <summary>
    /// Creates an instance of the <see cref="CreateTestQuestionModelValidator"/>.
    /// </summary>
    public CreateTestQuestionModelValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Answers).NotEmpty();
        RuleForEach(x => x.Answers).NotEmpty().SetValidator(new CreateTestAnswerModelValidator());
    }
}