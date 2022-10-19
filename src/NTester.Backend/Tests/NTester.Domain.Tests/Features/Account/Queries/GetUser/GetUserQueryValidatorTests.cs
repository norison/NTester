using FluentValidation.TestHelper;
using NTester.Domain.Features.Account.Queries.GetUser;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Account.Queries.GetUser;

[TestFixture]
public class GetUserQueryValidatorTests : ValidatorTestBase<GetUserQuery, GetUserQueryValidator>
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
        void Mutation(GetUserQuery command) => command.UserId = Guid.Empty;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.UserId);
    }
    
    protected override GetUserQuery CreateValidObject()
    {
        return new GetUserQuery
        {
            UserId = Guid.NewGuid()
        };
    }
}