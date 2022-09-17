using FluentValidation;

namespace NTester.Domain.Features.Tests.Commands.Create.Models;

/// <summary>
/// Validator for <see cref="CreateTestAnswerModel"/>.
/// </summary>
public class CreateTestAnswerModelValidator : AbstractValidator<CreateTestAnswerModel>
{
    /// <summary>
    /// Creates an instance of the <see cref="CreateTestAnswerModelValidator"/>.
    /// </summary>
    public CreateTestAnswerModelValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(100);
    }
}