using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace NTester.Domain.Extensions.Validation;

/// <summary>
/// Extensions to validate an authentication properties. 
/// </summary>
[ExcludeFromCodeCoverage]
public static class AuthValidationExtensions
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
    /// Rule builder for username.
    /// </summary>
    /// <param name="ruleBuilder">Rule builder.</param>
    public static IRuleBuilderOptions<T, string> UserName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().MinimumLength(2);
    }
    
    /// <summary>
    /// Rule builder for email.
    /// </summary>
    /// <param name="ruleBuilder">Rule builder.</param>
    public static IRuleBuilderOptions<T, string> Email<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().EmailAddress();
    }
    
    /// <summary>
    /// Rule builder for client name.
    /// </summary>
    /// <param name="ruleBuilder">Rule builder.</param>
    public static IRuleBuilderOptions<T, string> ClientName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty();
    }
    
    /// <summary>
    /// Rule builder for access token.
    /// </summary>
    /// <param name="ruleBuilder">Rule builder.</param>
    public static IRuleBuilderOptions<T, string> AccessToken<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty();
    }
    
    /// <summary>
    /// Rule builder for name.
    /// </summary>
    /// <param name="ruleBuilder">Rule builder.</param>
    public static IRuleBuilderOptions<T, string> Name<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().MinimumLength(2).MaximumLength(10).NoWhiteSpaces();
    }
    
    /// <summary>
    /// Rule builder for surname.
    /// </summary>
    /// <param name="ruleBuilder">Rule builder.</param>
    public static IRuleBuilderOptions<T, string> Surname<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().MinimumLength(2).MaximumLength(10).NoWhiteSpaces();
    }
}