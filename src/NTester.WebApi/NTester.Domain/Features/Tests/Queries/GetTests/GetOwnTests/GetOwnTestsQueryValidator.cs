using NTester.Domain.Extensions.Validation;

namespace NTester.Domain.Features.Tests.Queries.GetTests.GetOwnTests;

/// <summary>
/// Validator for <see cref="GetOwnTestsQuery"/>.
/// </summary>
public class GetOwnTestsQueryValidator : GetTestsQueryBaseValidator<GetOwnTestsQuery>
{
    /// <summary>
    /// Creates an instance of the <see cref="GetOwnTestsQueryValidator"/>.
    /// </summary>
    public GetOwnTestsQueryValidator()
    {
        RuleFor(x => x.UserId).UserId();
    }
}