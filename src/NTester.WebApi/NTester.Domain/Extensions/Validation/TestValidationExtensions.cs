using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace NTester.Domain.Extensions.Validation;

/// <summary>
/// Extensions to validate a test.
/// </summary>
[ExcludeFromCodeCoverage]
public static class TestValidationExtensions
{
    /// <summary>
    /// Rule builder for title.
    /// </summary>
    /// <param name="ruleBuilder">Rule builder.</param>
    public static IRuleBuilderOptions<T, string> Title<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().MaximumLength(50);
    }
    
    /// <summary>
    /// Rule builder for description.
    /// </summary>
    /// <param name="ruleBuilder">Rule builder.</param>
    public static IRuleBuilderOptions<T, string?> Description<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder.MaximumLength(200);
    }
}