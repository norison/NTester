using NTester.Domain.Extensions.Validation;
using NTester.Domain.Features.Tests.Queries.Common;

namespace NTester.Domain.Features.Tests.Queries.GetTests;

/// <summary>
/// Validator for <see cref="GetTestsQueryBase"/>.
/// </summary>
public abstract class GetTestsQueryBaseValidator<T> : PageQueryValidator<T> where T : GetTestsQueryBase
{
    /// <summary>
    /// Creates an instance of the <see cref="GetTestsQueryBaseValidator{T}"/>.
    /// </summary>
    protected GetTestsQueryBaseValidator()
    {
        RuleFor(x => x.UserId).UserId();
    }
}