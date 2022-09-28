using FluentValidation.TestHelper;
using NTester.Domain.Features.Tests.Commands.Create.Models;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Tests.Commands.Create.Models;

[TestFixture]
public class CreateTestQuestionModelValidatorTests
    : ValidatorTestBase<CreateTestQuestionModel, CreateTestQuestionModelValidator>
{
    [Test]
    public void Model_ValidProperties_ShouldNotHaveAnyValidationErrors()
    {
        // Act
        var result = Validate();

        // Arrange
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Test, TestCaseSource(nameof(ContentTestCases))]
    public void Content_InvalidValue_ShouldHaveValidationErrors(string content)
    {
        // Arrange
        void Mutation(CreateTestQuestionModel model) => model.Content = content;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(model => model.Content);
    }
    
    [Test, TestCaseSource(nameof(AnswersTestCases))]
    public void Answers_InvalidValue_ShouldHaveValidationErrors(IEnumerable<CreateTestAnswerModel> answers)
    {
        // Arrange
        void Mutation(CreateTestQuestionModel model) => model.Answers = answers;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(model => model.Answers);
    }
    
    protected override CreateTestQuestionModel CreateValidObject()
    {
        return new CreateTestQuestionModel
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
        };
    }
    
    private static IEnumerable<string> ContentTestCases => new List<string>
    {
        null!,
        "",
        new('a', 101)
    };
    
    private static IEnumerable<IEnumerable<CreateTestAnswerModel>> AnswersTestCases =>
        new List<IEnumerable<CreateTestAnswerModel>>
        {
            null!,
            new List<CreateTestAnswerModel>()
        };
}