using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NTester.Domain.Constants;
using NTester.Domain.Services.Cookie;
using NTester.Domain.Services.DateTime;
using NUnit.Framework;

namespace NTester.Domain.Tests.Services.Cookie;

[TestFixture]
public class CookieServiceTests
{
    private IHttpContextAccessor _contextAccessor;
    private IDateTimeService _dateTimeService;
    private CookieService _cookieService;

    [SetUp]
    public void SetUp()
    {
        _contextAccessor = Substitute.For<IHttpContextAccessor>();
        _dateTimeService = Substitute.For<IDateTimeService>();
        _cookieService = new CookieService(_contextAccessor, _dateTimeService);
    }

    [Test, AutoData]
    public void SetRefreshToken_ShouldSetRefreshTokenCookie(string refreshToken)
    {
        // Arrange
        CookieOptions capturedOptions = null!;
        _contextAccessor.HttpContext.Response.Cookies
            .WhenForAnyArgs(x => x.Append(default, default, default))
            .Do(x => capturedOptions = x.Arg<CookieOptions>());

        // Act
        _cookieService.SetRefreshToken(refreshToken);

        // Assert
        _contextAccessor.HttpContext.Response.Cookies
            .Received()
            .Append(CookieConstants.RefreshTokenCookieName, refreshToken, capturedOptions);

        capturedOptions.Should().NotBeNull();
        capturedOptions.Secure.Should().BeTrue();
        capturedOptions.HttpOnly.Should().BeTrue();
        capturedOptions.SameSite.Should().Be(SameSiteMode.None);
    }

    [Test, AutoData]
    public void RemoveRefreshToken_ShouldRemoveRefreshTokenCookie(DateTime dateTime)
    {
        // Arrange
        CookieOptions capturedOptions = null!;
        _contextAccessor.HttpContext.Response.Cookies
            .WhenForAnyArgs(x => x.Delete(default, default))
            .Do(x => capturedOptions = x.Arg<CookieOptions>());
        
        _dateTimeService.UtcNow.Returns(dateTime);
        
        // Act
        _cookieService.RemoveRefreshToken();

        // Assert
        _contextAccessor.HttpContext.Response.Cookies
            .Received()
            .Delete(CookieConstants.RefreshTokenCookieName, capturedOptions);

        capturedOptions.Secure.Should().BeTrue();
        capturedOptions.HttpOnly.Should().BeTrue();
        capturedOptions.SameSite.Should().Be(SameSiteMode.None);
        capturedOptions.Expires.Should().Be(dateTime.AddYears(-1));
    }

    [Test, AutoData]
    public void GetRefreshToken_ShouldReturnRefreshTokenFromCookie(string refreshToken)
    {
        // Arrange
        _contextAccessor.HttpContext.Request.Cookies[CookieConstants.RefreshTokenCookieName].Returns(refreshToken);
        
        // Act
        var result = _cookieService.GetRefreshToken();

        // Assert
        result.Should().Be(refreshToken);
    }
}