using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace NTester.Domain.Extensions.Validation;

/// <summary>
/// Extensions for the common scenarios.
/// </summary>
[ExcludeFromCodeCoverage]
public static class CommonValidationExtensions
{
    /// <summary>
    /// Rule builder for user id.
    /// </summary>
    /// <param name="ruleBuilder">Rule builder.</param>
    public static IRuleBuilderOptions<T, Guid> UserId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder.NotEmpty();
    }
    
    /// <summary>
    /// Rule builder for string to validate the white spaces.
    /// </summary>
    /// <param name="ruleBuilder">Rule builder.</param>
    public static IRuleBuilderOptions<T, string> NoWhiteSpaces<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Matches(@"\A\S+\z");
    }
}