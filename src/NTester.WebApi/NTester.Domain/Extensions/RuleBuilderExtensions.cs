using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace NTester.Domain.Extensions;

/// <summary>
/// Extension methods for rule builder.
/// </summary>
[ExcludeFromCodeCoverage]
public static class RuleBuilderExtensions
{
    /// <summary>
    /// Rule builder for password.
    /// </summary>
    /// <param name="ruleBuilder">Rule builder.</param>
    /// <param name="requiredLength">Minimum length a password must be.</param>
    /// <param name="requireDigits">If passwords must contain a digit.</param>
    /// <param name="requireLowercase">If passwords must contain a lower case ASCII character.</param>
    /// <param name="requireUppercase">If passwords must contain a upper case ASCII character.</param>
    public static IRuleBuilderOptions<T, string> Password<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        int requiredLength = 2,
        bool requireDigits = false,
        bool requireLowercase = false,
        bool requireUppercase = false)
    {
        var options = ruleBuilder.NotEmpty().MinimumLength(requiredLength).NoWhiteSpaces();

        if (requireUppercase)
        {
            options.Matches("[A-Z]");
        }

        if (requireLowercase)
        {
            options.Matches("[a-z]");
        }

        if (requireDigits)
        {
            options.Matches("[0-9]");
        }

        return options;
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