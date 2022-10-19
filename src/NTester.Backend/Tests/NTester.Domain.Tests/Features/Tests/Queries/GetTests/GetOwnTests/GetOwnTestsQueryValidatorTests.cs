using FluentValidation.TestHelper;
using NTester.Domain.Features.Tests.Queries.GetTests.GetOwnTests;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Tests.Queries.GetTests.GetOwnTests;

[TestFixture]
public class GetOwnTestsQueryValidatorTests : ValidatorTestBase<GetOwnTestsQuery, GetOwnTestsQueryValidator>
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
        void Mutation(GetOwnTestsQuery query) => query.UserId = Guid.Empty;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(query => query.UserId);
    }

    protected override GetOwnTestsQuery CreateValidObject()
    {
        return new GetOwnTestsQuery
        {
            PageNumber = 1,
            PageSize = 5,
            UserId = Guid.NewGuid(),
            Title = "title",
            Published = false
        };
    }
}