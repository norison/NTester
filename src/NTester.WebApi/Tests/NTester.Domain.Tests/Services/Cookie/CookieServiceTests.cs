﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NTester.Domain.Services.Cookie;
using NUnit.Framework;

namespace NTester.Domain.Tests.Services.Cookie;

[TestFixture]
public class CookieServiceTests
{
    private IHttpContextAccessor _contextAccessor;
    private CookieService _cookieService;

    [SetUp]
    public void SetUp()
    {
        _contextAccessor = Substitute.For<IHttpContextAccessor>();
        _cookieService = new CookieService(_contextAccessor);
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
            .Append(CookieService.RefreshTokenCookieName, refreshToken, capturedOptions);

        capturedOptions.Should().NotBeNull();
        capturedOptions.Secure.Should().BeTrue();
        capturedOptions.HttpOnly.Should().BeTrue();
        capturedOptions.SameSite.Should().Be(SameSiteMode.Strict);
    }

    [Test]
    public void RemoveRefreshToken_ShouldRemoveRefreshTokenCookie()
    {
        // Act
        _cookieService.RemoveRefreshToken();

        // Assert
        _contextAccessor.HttpContext.Response.Cookies
            .Received()
            .Delete(CookieService.RefreshTokenCookieName);
    }

    [Test, AutoData]
    public void GetRefreshToken_ShouldReturnRefreshTokenFromCookie(string refreshToken)
    {
        // Arrange
        _contextAccessor.HttpContext.Request.Cookies[CookieService.RefreshTokenCookieName].Returns(refreshToken);
        
        // Act
        var result = _cookieService.GetRefreshToken();

        // Assert
        result.Should().Be(refreshToken);
    }
}