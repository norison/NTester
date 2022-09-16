using FluentValidation.TestHelper;
using NTester.Domain.Features.Auth.Commands.Refresh;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Auth.Refresh;

[TestFixture]
public class RefreshCommandValidatorTests: ValidatorTestBase<RefreshCommand, RefreshCommandValidator>
{
    [Test]
    public void Command_ValidProperties_ShouldNotHaveAnyValidationErrors()
    {
        // Act
        var result = Validate();

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    public void AccessToken_InvalidValue_ShouldHaveValidationErrors(string accessToken)
    {
        // Arrange
        void Mutation(RefreshCommand command) => command.AccessToken = accessToken;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.AccessToken);
    }
    
    protected override RefreshCommand CreateValidObject()
    {
        return new RefreshCommand
        {
            AccessToken = "token"
        };
    }
}