using FluentValidation;
using NTester.Domain.Features.Tests.Queries.GetTests.GetPublicTests;

namespace NTester.Domain.Features.Tests.Queries.Common;

/// <summary>
/// Validator for <see cref="PageQuery"/>.
/// </summary>
public class PageQueryValidator : AbstractValidator<GetPublicTestsQuery>
{
    /// <summary>
    /// Creates an instance of the <see cref="PageQueryValidator"/>.
    /// </summary>
    public PageQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}