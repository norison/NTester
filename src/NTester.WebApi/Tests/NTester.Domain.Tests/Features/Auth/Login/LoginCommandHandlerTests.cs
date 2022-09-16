using FluentAssertions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NTester.DataAccess.Entities;
using NTester.DataContracts.Auth;
using NTester.Domain.Exceptions.Auth;
using NTester.Domain.Features.Auth.Commands.Login;
using NTester.Domain.Services.Auth;
using NTester.Domain.Services.Cookie;
using NTester.Domain.Services.SignInManager;
using NTester.Domain.Services.UserManager;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Auth.Login;

[TestFixture]
public class LoginCommandHandlerTests
{
    private IUserManager _userManager;
    private ISignInManager _signInManager;
    private IAuthService _authService;
    private ICookieService _cookieService;
    private LoginCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _userManager = Substitute.For<IUserManager>();
        _signInManager = Substitute.For<ISignInManager>();
        _authService = Substitute.For<IAuthService>();
        _cookieService = Substitute.For<ICookieService>();

        _handler = new LoginCommandHandler(_userManager, _signInManager, _authService, _cookieService);
    }

    [Test, AutoDataExt]
    public async Task Handle_InvalidPassword_ShouldThrowAnException(LoginCommand loginCommand)
    {
        // Act/Assert
        await _handler
            .Invoking(x => x.Handle(loginCommand, CancellationToken.None))
            .Should()
            .ThrowAsync<InvalidUserNameOrPasswordException>();

        await _userManager.Received().FindByNameAsync(loginCommand.UserName);
    }

    [Test, AutoDataExt]
    public async Task Handle_UserDoesNotExist_ShouldThrowAnException(LoginCommand loginCommand, UserEntity user)
    {
        // Arrange
        UserEntity capturedUser = null!;
        var signInResult = SignInResult.Failed;

        _userManager.FindByNameAsync(default!).ReturnsForAnyArgs(user);
        _signInManager
            .CheckPasswordSignInAsync(default!, default!, default)
            .ReturnsForAnyArgs(signInResult)
            .AndDoes(x => capturedUser = x.Arg<UserEntity>());

        // Act/Assert
        await _handler
            .Invoking(x => x.Handle(loginCommand, CancellationToken.None))
            .Should()
            .ThrowAsync<InvalidUserNameOrPasswordException>();

        await _userManager.Received().FindByNameAsync(loginCommand.UserName);
        await _signInManager.Received().CheckPasswordSignInAsync(Arg.Any<UserEntity>(), loginCommand.Password, false);

        capturedUser.Should().Be(user);
    }

    [Test, AutoDataExt]
    public async Task Handle_NoErrors_ShouldReturnCorrectResult(
        LoginCommand loginCommand,
        UserEntity user,
        AuthResponse authResponse)
    {
        // Arrange
        UserEntity capturedUser = null!;
        var signInResult = SignInResult.Success;

        _userManager.FindByNameAsync(default!).ReturnsForAnyArgs(user);
        _signInManager
            .CheckPasswordSignInAsync(default!, default!, default)
            .ReturnsForAnyArgs(signInResult)
            .AndDoes(x => capturedUser = x.Arg<UserEntity>());

        _authService.AuthenticateUserAsync((UserEntity)default!, default).ReturnsForAnyArgs(authResponse);

        // Act
        var result = await _handler.Handle(loginCommand, CancellationToken.None);
        
        // Assert
        await _userManager.Received().FindByNameAsync(loginCommand.UserName);
        await _signInManager.Received().CheckPasswordSignInAsync(Arg.Any<UserEntity>(), loginCommand.Password, false);
        
        _cookieService.Received().SetRefreshToken(result.RefreshToken);

        result.Should().Be(authResponse);
        capturedUser.Should().Be(user);
    }
}