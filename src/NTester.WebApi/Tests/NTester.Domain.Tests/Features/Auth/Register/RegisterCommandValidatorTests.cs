using FluentValidation.TestHelper;
using NTester.Domain.Features.Auth.Commands.Register;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Auth.Register;

[TestFixture]
public class RegisterCommandValidatorTests : ValidatorTestBase<RegisterCommand, RegisterCommandValidator>
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
    [TestCase("a")]
    public void UserName_InvalidValue_ShouldHaveValidationErrors(string userName)
    {
        // Arrange
        void Mutation(RegisterCommand command) => command.UserName = userName;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.UserName);
    }
    
    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("test.email@")]
    [TestCase("@test.email")]
    public void Email_InvalidValue_ShouldHaveValidationErrors(string email)
    {
        // Arrange
        void Mutation(RegisterCommand command) => command.Email = email;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Email);
    }
    
    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("a")]
    [TestCase("a a")]
    public void Password_InvalidValue_ShouldHaveValidationErrors(string password)
    {
        // Arrange
        void Mutation(RegisterCommand command) => command.Password = password;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Password);
    }
    
    [TestCase(null)]
    [TestCase("")]
    [TestCase("a")]
    [TestCase("a a")]
    public void Name_InvalidValue_ShouldHaveValidationErrors(string name)
    {
        // Arrange
        void Mutation(RegisterCommand command) => command.Name = name;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Name);
    }
    
    [TestCase(null)]
    [TestCase("")]
    [TestCase("a")]
    [TestCase("a a")]
    public void Surname_InvalidValue_ShouldHaveValidationErrors(string surname)
    {
        // Arrange
        void Mutation(RegisterCommand command) => command.Surname = surname;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Surname);
    }
    
    [Test]
    public void ClientId_InvalidValue_ShouldHaveValidationErrors()
    {
        // Arrange
        void Mutation(RegisterCommand command) => command.ClientId = Guid.Empty;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.ClientId);
    }

    protected override RegisterCommand CreateValidObject()
    {
        return new RegisterCommand
        {
            UserName = "UserName",
            Email = "test.email@gmail.com",
            Password = "Password",
            Name = "Name",
            Surname = "Surname",
            ClientId = Guid.NewGuid()
        };
    }
}