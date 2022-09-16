using Microsoft.AspNetCore.Http;
using NTester.Domain.Constants;

namespace NTester.Domain.Services.Cookie;

/// <inheritdoc cref="ICookieService"/>
public class CookieService : ICookieService
{
    private readonly IHttpContextAccessor _contextAccessor;

    /// <summary>
    /// Creates an instance of the cookie service.
    /// </summary>
    /// <param name="contextAccessor">Accessor to the http context.</param>
    public CookieService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    /// <inheritdoc cref="ICookieService.SetRefreshToken"/>
    public void SetRefreshToken(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.Strict
        };

        _contextAccessor.HttpContext.Response.Cookies.Append(
            CookieConstants.RefreshTokenCookieName,
            refreshToken,
            cookieOptions);
    }

    /// <inheritdoc cref="ICookieService.RemoveRefreshToken"/>
    public void RemoveRefreshToken()
    {
        _contextAccessor.HttpContext.Response.Cookies.Delete(CookieConstants.RefreshTokenCookieName);
    }

    /// <inheritdoc cref="ICookieService.GetRefreshToken"/>
    public string? GetRefreshToken()
    {
        return _contextAccessor.HttpContext.Request.Cookies[CookieConstants.RefreshTokenCookieName];
    }
}