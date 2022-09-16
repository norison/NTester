using FluentValidation.TestHelper;
using NTester.Domain.Features.Account.GetUser;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Account.GetUser;

[TestFixture]
public class GetUserCommandValidatorTests : ValidatorTestBase<GetUserCommand, GetUserCommandValidator>
{
    [Test]
    public void Command_ValidProperties_ShouldNotHaveAnyValidationErrors()
    {
        // Act
        var result = Validate();

        // Arrange
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Test]
    public void UserId_InvalidValue_ShouldHaveValidationErrors()
    {
        // Arrange
        void Mutation(GetUserCommand command) => command.UserId = Guid.Empty;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.UserId);
    }
    
    protected override GetUserCommand CreateValidObject()
    {
        return new GetUserCommand
        {
            UserId = Guid.NewGuid()
        };
    }
}