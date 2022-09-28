using FluentValidation;

namespace NTester.Domain.Features.Tests.Queries.Common;

/// <summary>
/// Validator for <see cref="PageQuery"/>.
/// </summary>
public class PageQueryValidator<T> : AbstractValidator<T> where T : PageQuery
{
    /// <summary>
    /// Creates an instance of the <see cref="PageQueryValidator{T}"/>.
    /// </summary>
    public PageQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}