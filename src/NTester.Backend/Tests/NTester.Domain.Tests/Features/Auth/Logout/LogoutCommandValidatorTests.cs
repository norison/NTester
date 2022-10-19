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
    [TestCase(null)]
    [TestCase("")]
    public void ClientName_InvalidValue_ShouldHaveValidationErrors(string clientName)
    {
        // Arrange
        void Mutation(LogoutCommand command) => command.ClientName = clientName;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.ClientName);
    }
    
    protected override LogoutCommand CreateValidObject()
    {
        return new LogoutCommand
        {
            ClientName = "client",
            UserId = Guid.NewGuid()
        };
    }
}