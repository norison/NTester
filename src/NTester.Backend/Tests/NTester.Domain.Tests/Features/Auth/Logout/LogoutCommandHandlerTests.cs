using FluentAssertions;
using NSubstitute;
using NTester.Domain.Exceptions.Auth;
using NTester.Domain.Features.Auth.Commands.Logout;
using NTester.Domain.Services.Auth;
using NTester.Domain.Services.Cookie;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Auth.Logout;

[TestFixture]
public class LogoutCommandHandlerTests
{
    private IAuthService _authService;
    private ICookieService _cookieService;
    private LogoutCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _authService = Substitute.For<IAuthService>();
        _cookieService = Substitute.For<ICookieService>();

        _handler = new LogoutCommandHandler(_authService, _cookieService);
    }

    [Test, AutoDataExt]
    public async Task Handle_RefreshTokenNotFound_ShouldThrowAnException(LogoutCommand logoutCommand)
    {
        // Act/Assert
        await _handler
            .Invoking(x => x.Handle(logoutCommand, CancellationToken.None))
            .Should()
            .ThrowAsync<RefreshTokenWasNotProvidedException>();

        _cookieService.Received().GetRefreshToken();
    }
    
    [Test, AutoDataExt]
    public async Task Handle_NoErrors_ShouldSucceed(LogoutCommand logoutCommand, string refreshToken)
    {
        // Arrange
        _cookieService.GetRefreshToken().Returns(refreshToken);

        // Act
        await _handler.Handle(logoutCommand, CancellationToken.None);
        
        // Assert
        _cookieService.Received().GetRefreshToken();
        _cookieService.Received().RemoveRefreshToken();

        await _authService.Received().RevokeRefreshTokenAsync(refreshToken, logoutCommand.UserId, logoutCommand.ClientName);
    }
}