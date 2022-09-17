using FluentValidation.TestHelper;
using NTester.Domain.Features.Tests.Commands.Create;
using NTester.Domain.Features.Tests.Commands.Create.Models;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Tests;

[TestFixture]
public class CreateTestCommandValidatorTests : ValidatorTestBase<CreateTestCommand, CreateTestCommandValidator>
{
    [Test]
    public void Command_ValidProperties_ShouldNotHaveAnyValidationErrors()
    {
        // Act
        var result = Validate();

        // Arrange
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test, TestCaseSource(nameof(TitleTestCases))]
    public void Title_InvalidValue_ShouldHaveValidationErrors(string title)
    {
        // Arrange
        void Mutation(CreateTestCommand command) => command.Title = title;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Title);
    }

    [Test, TestCaseSource(nameof(DescriptionTestCases))]
    public void Description_InvalidValue_ShouldHaveValidationErrors(string description)
    {
        // Arrange
        void Mutation(CreateTestCommand command) => command.Description = description;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Description);
    }

    [Test, TestCaseSource(nameof(QuestionsTestCases))]
    public void Questions_InvalidValue_ShouldHaveValidationErrors(IEnumerable<CreateTestQuestionModel> questions)
    {
        // Arrange
        void Mutation(CreateTestCommand command) => command.Questions = questions;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Questions);
    }
    
    [Test]
    public void UserId_InvalidValue_ShouldHaveValidationErrors()
    {
        // Arrange
        void Mutation(CreateTestCommand command) => command.UserId = Guid.Empty;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.UserId);
    }

    protected override CreateTestCommand CreateValidObject()
    {
        return new CreateTestCommand
        {
            UserId = Guid.NewGuid(),
            Title = "test title",
            Description = "test description",
            Questions = new List<CreateTestQuestionModel>
            {
                new()
                {
                    Content = "question content",
                    Answers = new List<CreateTestAnswerModel>
                    {
                        new()
                        {
                            Content = "answer content",
                            IsCorrect = false
                        }
                    }
                }
            }
        };
    }

    private static IEnumerable<string> TitleTestCases => new List<string>
    {
        null!,
        "",
        new('a', 51)
    };

    private static IEnumerable<string> DescriptionTestCases => new List<string>
    {
        null!,
        "",
        new('a', 201)
    };

    private static IEnumerable<IEnumerable<CreateTestQuestionModel>> QuestionsTestCases =>
        new List<IEnumerable<CreateTestQuestionModel>>
        {
            null!,
            new List<CreateTestQuestionModel>()
        };
}