using FluentValidation.TestHelper;
using NTester.Domain.Features.Auth.Commands.Login;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Auth.Login;

[TestFixture]
public class LoginCommandValidatorTests : ValidatorTestBase<LoginCommand, LoginCommandValidator>
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
    [TestCase(null)]
    [TestCase("")]
    [TestCase("a")]
    public void UserName_InvalidValue_ShouldHaveValidationErrors(string userName)
    {
        // Arrange
        void Mutation(LoginCommand command) => command.UserName = userName;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.UserName);
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("a")]
    [TestCase("a a")]
    public void Password_InvalidValue_ShouldHaveValidationErrors(string password)
    {
        // Arrange
        void Mutation(LoginCommand command) => command.Password = password;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Password);
    }

    [Test]
    public void ClientId_InvalidValue_ShouldHaveValidationErrors()
    {
        // Arrange
        void Mutation(LoginCommand command) => command.ClientId = Guid.Empty;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.ClientId);
    }

    protected override LoginCommand CreateValidObject()
    {
        return new LoginCommand
        {
            UserName = "username",
            Password = "password",
            ClientId = Guid.NewGuid()
        };
    }
}