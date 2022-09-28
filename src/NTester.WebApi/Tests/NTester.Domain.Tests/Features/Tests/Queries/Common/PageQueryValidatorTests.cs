using FluentValidation.TestHelper;
using NTester.Domain.Features.Tests.Queries.Common;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Tests.Queries.Common;

[TestFixture]
public class PageQueryValidatorTests : ValidatorTestBase<PageQuery, PageQueryValidator<PageQuery>>
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
    [TestCase(0)]
    [TestCase(-1)]
    public void PageNumber_InvalidValue_ShouldHaveValidationErrors(int pageNumber)
    {
        // Arrange
        void Mutation(PageQuery query) => query.PageNumber = pageNumber;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(query => query.PageNumber);
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void PageSize_InvalidValue_ShouldHaveValidationErrors(int pageSize)
    {
        // Arrange
        void Mutation(PageQuery query) => query.PageSize = pageSize;

        // Act
        var result = Validate(Mutation);

        // Assert
        result.ShouldHaveValidationErrorFor(query => query.PageSize);
    }

    protected override PageQuery CreateValidObject()
    {
        return new PageQuery
        {
            PageNumber = 1,
            PageSize = 5
        };
    }
}