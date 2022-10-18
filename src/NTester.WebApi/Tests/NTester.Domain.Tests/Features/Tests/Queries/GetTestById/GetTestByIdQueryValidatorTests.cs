using FluentValidation.TestHelper;
using NTester.Domain.Features.Tests.Queries.GetTestById;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Tests.Queries.GetTestById;

[TestFixture]
public class GetTestByIdQueryValidatorTests : ValidatorTestBase<GetTestByIdQuery, GetTestByIdQueryValidator>
{
    [Test]
    public void Command_ValidProperties_ShouldNotHaveAnyValidationErrors()
    {
        // Act
        var result = Validate();

        // Arrange
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    protected override GetTestByIdQuery CreateValidObject()
    {
        return new GetTestByIdQuery
        {
            UserId = Guid.NewGuid(),
            Id = Guid.NewGuid()
        };
    }
}