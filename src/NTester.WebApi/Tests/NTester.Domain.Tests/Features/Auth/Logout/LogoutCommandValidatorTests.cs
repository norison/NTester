using FluentValidation.TestHelper;
using NTester.Domain.Features.Auth.Commands.Logout;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Auth.Logout;

[TestFixture]
public class LogoutCommandValidatorTests : ValidatorTestBase<LogoutCommand, LogoutCommandValidator>
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
    public void UserId_InvalidValue_ShouldHaveValidationErrors()
    {
        // Arrange
        void Mutation(LogoutCommand command) => command.UserId = Guid.Empty;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.UserId);
    }
    
    [Test]
    public void ClientId_InvalidValue_ShouldHaveValidationErrors()
    {
        // Arrange
        void Mutation(LogoutCommand command) => command.ClientId = Guid.Empty;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.ClientId);
    }
    
    protected override LogoutCommand CreateValidObject()
    {
        return new LogoutCommand
        {
            ClientId = Guid.NewGuid(),
            UserId = Guid.NewGuid()
        };
    }
}