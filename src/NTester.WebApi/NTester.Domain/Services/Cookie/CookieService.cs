using Microsoft.AspNetCore.Http;

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

    private const string RefreshTokenCookieName = "Refresh-Token";

    /// <inheritdoc cref="ICookieService.SetRefreshToken"/>
    public void SetRefreshToken(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.Strict
        };

        _contextAccessor.HttpContext.Response.Cookies.Append(RefreshTokenCookieName, refreshToken, cookieOptions);
    }

    /// <inheritdoc cref="ICookieService.RemoveRefreshToken"/>
    public void RemoveRefreshToken()
    {
        _contextAccessor.HttpContext.Response.Cookies.Delete(RefreshTokenCookieName);
    }

    /// <inheritdoc cref="ICookieService.GetRefreshToken"/>
    public string? GetRefreshToken()
    {
        return _contextAccessor.HttpContext.Request.Cookies[RefreshTokenCookieName];
    }
}