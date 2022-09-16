using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using NSubstitute;
using NTester.DataAccess.Entities;
using NTester.DataAccess.Services.Transaction;
using NTester.DataContracts.Auth;
using NTester.Domain.Exceptions.Auth;
using NTester.Domain.Exceptions.Common;
using NTester.Domain.Features.Auth.Commands.Register;
using NTester.Domain.Services.Auth;
using NTester.Domain.Services.Cookie;
using NTester.Domain.Services.UserManager;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Auth.Register;

[TestFixture]
public class RegisterCommandHandlerTests
{
    private IUserManager _userManager;
    private IAuthService _authService;
    private ICookieService _cookieService;
    private ITransactionFactory _transactionFactory;
    private IDbContextTransaction _dbContextTransaction;
    private RegisterCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _userManager = Substitute.For<IUserManager>();
        _authService = Substitute.For<IAuthService>();
        _cookieService = Substitute.For<ICookieService>();
        _transactionFactory = Substitute.For<ITransactionFactory>();
        _dbContextTransaction = Substitute.For<IDbContextTransaction>();

        _transactionFactory.CreateTransactionAsync(default).ReturnsForAnyArgs(_dbContextTransaction);

        _handler = new RegisterCommandHandler(_userManager, _authService, _cookieService, _transactionFactory);
    }

    [Test, AutoDataExt]
    public async Task Handle_UserAlreadyExists_ShouldThrowAnException(
        RegisterCommand registerCommand,
        UserEntity user)
    {
        // Arrange
        _userManager.FindByNameAsync(default!).ReturnsForAnyArgs(user);

        // Act/Assert
        await _handler
            .Invoking(x => x.Handle(registerCommand, CancellationToken.None))
            .Should()
            .ThrowAsync<InvalidUserNameOrPasswordException>();

        await _userManager.Received().FindByNameAsync(registerCommand.UserName);
    }

    [Test, AutoDataExt]
    public async Task Handle_FailedToCreateAUser_ShouldThrowAnException(
        RegisterCommand registerCommand,
        IdentityError identityError)
    {
        // Arrange
        UserEntity capturedUser = null!;

        var identityResult = IdentityResult.Failed(identityError);

        _userManager
            .CreateAsync(default!, default!)
            .ReturnsForAnyArgs(identityResult)
            .AndDoes(x => capturedUser = x.Arg<UserEntity>());

        // Act/Assert
        await _handler
            .Invoking(x => x.Handle(registerCommand, CancellationToken.None))
            .Should()
            .ThrowAsync<NonGeneralException>()
            .WithMessage(identityError.Description);

        await _transactionFactory.Received().CreateTransactionAsync(CancellationToken.None);
        await _dbContextTransaction.Received().DisposeAsync();

        await AssertUserManager(registerCommand, capturedUser);
        AssertUser(registerCommand, capturedUser);
    }

    [Test, AutoDataExt]
    public async Task Handle_NoErrors_ShouldReturnCorrectResult(
        RegisterCommand registerCommand,
        UserEntity user,
        AuthResponse authResponse)
    {
        // Arrange
        UserEntity capturedUser = null!;

        var identityResult = IdentityResult.Success;

        _userManager
            .CreateAsync(default!, default!)
            .ReturnsForAnyArgs(identityResult)
            .AndDoes(x => capturedUser = x.Arg<UserEntity>());

        _authService.AuthenticateUserAsync((UserEntity)default!, default).ReturnsForAnyArgs(authResponse);

        // Act
        var result = await _handler.Handle(registerCommand, CancellationToken.None);

        // Assert
        result.Should().Be(authResponse);

        _cookieService.Received().SetRefreshToken(result.RefreshToken);

        await _transactionFactory.Received().CreateTransactionAsync(CancellationToken.None);
        await _dbContextTransaction.Received().CommitAsync();
        await _dbContextTransaction.Received().DisposeAsync();

        await AssertUserManager(registerCommand, capturedUser);
        AssertUser(registerCommand, capturedUser);
    }

    private async Task AssertUserManager(RegisterCommand registerCommand, UserEntity user)
    {
        await _userManager.Received().FindByNameAsync(registerCommand.UserName);
        await _userManager.Received().CreateAsync(user, registerCommand.Password);
    }

    private static void AssertUser(RegisterCommand registerCommand, UserEntity user)
    {
        user.Id.Should().NotBeEmpty();
        user.UserName.Should().Be(registerCommand.UserName);
        user.Email.Should().Be(registerCommand.Email);
        user.Name.Should().Be(registerCommand.Name);
        user.Surname.Should().Be(registerCommand.Surname);
    }
}