using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NTester.Domain.Exceptions;
using NTester.Domain.Exceptions.Codes;
using NTester.Domain.Services.DateTime;

namespace NTester.Domain.Services.Token;

/// <inheritdoc cref="ITokenService"/>
[ExcludeFromCodeCoverage]
public class TokenService : ITokenService
{
    private readonly IDateTimeService _dateTimeService;
    private readonly JwtSettings _jwtSettings;

    private const string ErrorMessageInvalidAccessToken =
        "The access token has invalid values or cannot be verified by the secret.";

    /// <summary>
    /// Creates an instance of the token service.
    /// </summary>
    /// <param name="dateTimeService">Service for date and time generation.</param>
    /// <param name="jwtSettings">Settings of the token.</param>
    public TokenService(IDateTimeService dateTimeService, IOptions<JwtSettings> jwtSettings)
    {
        _dateTimeService = dateTimeService;
        _jwtSettings = jwtSettings.Value;
    }

    /// <inheritdoc cref="ITokenService.GenerateAccessToken"/>
    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

        var jwt = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            notBefore: _dateTimeService.UtcNow,
            expires: _dateTimeService.UtcNow.Add(_jwtSettings.LifeTime),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    /// <inheritdoc cref="ITokenService.GenerateRefreshToken"/>
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <inheritdoc cref="ITokenService.GetPrincipalFromExpiredAccessToken"/>
    public ClaimsPrincipal GetPrincipalFromExpiredAccessToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            jwtSecurityToken.Header.Alg != SecurityAlgorithms.HmacSha256)
        {
            throw new ValidationException((int)AuthCodes.InvalidAccessToken, ErrorMessageInvalidAccessToken);
        }

        return principal;
    }
}