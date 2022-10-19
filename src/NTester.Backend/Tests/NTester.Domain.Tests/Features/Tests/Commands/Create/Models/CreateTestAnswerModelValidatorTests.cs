using FluentValidation.TestHelper;
using NTester.Domain.Features.Tests.Commands.Create.Models;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Tests.Commands.Create.Models;

[TestFixture]
public class
    CreateTestAnswerModelValidatorTests : ValidatorTestBase<CreateTestAnswerModel, CreateTestAnswerModelValidator>
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
        void Mutation(CreateTestAnswerModel model) => model.Content = content;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(model => model.Content);
    }

    protected override CreateTestAnswerModel CreateValidObject()
    {
        return new CreateTestAnswerModel
        {
            Content = "answer test",
            IsCorrect = true
        };
    }
    
    private static IEnumerable<string> ContentTestCases => new List<string>
    {
        null!,
        "",
        new('a', 101)
    };
}