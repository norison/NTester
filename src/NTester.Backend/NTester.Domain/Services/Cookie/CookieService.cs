using Microsoft.AspNetCore.Http;
using NTester.Domain.Constants;
using NTester.Domain.Services.DateTime;

namespace NTester.Domain.Services.Cookie;

/// <inheritdoc cref="ICookieService"/>
public class CookieService : ICookieService
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IDateTimeService _dateTimeService;

    /// <summary>
    /// Creates an instance of the cookie service.
    /// </summary>
    /// <param name="contextAccessor">Accessor to the http context.</param>
    /// <param name="dateTimeService">Date and time service.</param>
    public CookieService(IHttpContextAccessor contextAccessor, IDateTimeService dateTimeService)
    {
        _contextAccessor = contextAccessor;
        _dateTimeService = dateTimeService;
    }

    /// <inheritdoc cref="ICookieService.SetRefreshToken"/>
    public void SetRefreshToken(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.None
        };

        _contextAccessor.HttpContext.Response.Cookies.Append(
            CookieConstants.RefreshTokenCookieName,
            refreshToken,
            cookieOptions);
    }

    /// <inheritdoc cref="ICookieService.RemoveRefreshToken"/>
    public void RemoveRefreshToken()
    {
        var cookieOptions = new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Expires = _dateTimeService.UtcNow.AddYears(-1)
        };

        _contextAccessor.HttpContext.Response.Cookies.Delete(CookieConstants.RefreshTokenCookieName, cookieOptions);
    }

    /// <inheritdoc cref="ICookieService.GetRefreshToken"/>
    public string? GetRefreshToken()
    {
        return _contextAccessor.HttpContext.Request.Cookies[CookieConstants.RefreshTokenCookieName];
    }
}