using FluentAssertions;
using NSubstitute;
using NTester.DataContracts.Auth;
using NTester.Domain.Exceptions.Auth;
using NTester.Domain.Features.Auth.Commands.Refresh;
using NTester.Domain.Services.Auth;
using NTester.Domain.Services.Cookie;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Auth.Refresh;

[TestFixture]
public class RefreshCommandHandlerTests
{
    private IAuthService _authService;
    private ICookieService _cookieService;
    private RefreshCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _authService = Substitute.For<IAuthService>();
        _cookieService = Substitute.For<ICookieService>();

        _handler = new RefreshCommandHandler(_authService, _cookieService);
    }

    [Test, AutoDataExt]
    public async Task Handle_RefreshTokenNotFound_ShouldThrowAnException(RefreshCommand refreshCommand)
    {
        // Act/Assert
        await _handler
            .Invoking(x => x.Handle(refreshCommand, CancellationToken.None))
            .Should()
            .ThrowAsync<RefreshTokenWasNotProvidedException>();

        _cookieService.Received().GetRefreshToken();
    }

    [Test, AutoDataExt]
    public async Task Handle_NoErrors_ShouldSucceed(
        RefreshCommand refreshCommand,
        AuthResponse authResponse,
        string refreshToken)
    {
        // Arrange
        _cookieService.GetRefreshToken().Returns(refreshToken);
        _authService.AuthenticateUserAsync((string)default!, default!).ReturnsForAnyArgs(authResponse);

        // Act
        var result = await _handler.Handle(refreshCommand, CancellationToken.None);

        // Assert
        _cookieService.Received().GetRefreshToken();
        _cookieService.Received().SetRefreshToken(result.RefreshToken);

        await _authService.Received().AuthenticateUserAsync(refreshCommand.AccessToken, refreshToken);

        result.Should().Be(authResponse);
    }
}