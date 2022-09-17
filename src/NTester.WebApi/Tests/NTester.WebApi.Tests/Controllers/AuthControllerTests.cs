using AutoFixture.NUnit3;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NTester.DataContracts.Auth;
using NTester.DataContracts.Auth.Login;
using NTester.DataContracts.Auth.Refresh;
using NTester.DataContracts.Auth.Register;
using NTester.Domain.Features.Auth.Commands.Login;
using NTester.Domain.Features.Auth.Commands.Logout;
using NTester.Domain.Features.Auth.Commands.Refresh;
using NTester.Domain.Features.Auth.Commands.Register;
using NTester.WebApi.Controllers;
using NTester.WebApi.Tests.Controllers.Base;
using NUnit.Framework;

namespace NTester.WebApi.Tests.Controllers;

[TestFixture]
public class AuthControllerTests : ControllerTestsBase
{
    private IMediator _mediator;
    private IMapper _mapper;
    private AuthController _authController;

    [SetUp]
    public void SetUp()
    {
        _mediator = Substitute.For<IMediator>();
        _mapper = Substitute.For<IMapper>();

        _authController = new AuthController(_mediator, _mapper);
    }

    [Test, AutoData]
    public async Task LoginAsync_ShouldReturnCorrectResult(
        LoginRequest loginRequest,
        LoginCommand loginCommand,
        AuthResponse authResponse)
    {
        // Arrange
        _mapper.Map<LoginCommand>(default).ReturnsForAnyArgs(loginCommand);
        _mediator.Send((LoginCommand)default!).ReturnsForAnyArgs(authResponse);

        // Act
        var result = await _authController.LoginAsync(loginRequest);

        // Assert
        _mapper.Received().Map<LoginCommand>(loginRequest);
        await _mediator.Received().Send(loginCommand);
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(authResponse);
    }

    [Test, AutoData]
    public async Task RegisterAsync_ShouldReturnCorrectResult(
        RegisterRequest registerRequest,
        RegisterCommand registerCommand,
        AuthResponse authResponse)
    {
        // Arrange
        _mapper.Map<RegisterCommand>(default).ReturnsForAnyArgs(registerCommand);
        _mediator.Send((RegisterCommand)default!).ReturnsForAnyArgs(authResponse);

        // Act
        var result = await _authController.RegisterAsync(registerRequest);

        // Assert
        _mapper.Received().Map<RegisterCommand>(registerRequest);
        await _mediator.Received().Send(registerCommand);
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(authResponse);
    }

    [Test, AutoData]
    public async Task RefreshAsync_ShouldReturnCorrectResult(
        RefreshRequest refreshRequest,
        RefreshCommand refreshCommand,
        AuthResponse authResponse)
    {
        // Arrange
        _mapper.Map<RefreshCommand>(default).ReturnsForAnyArgs(refreshCommand);
        _mediator.Send((RefreshCommand)default!).ReturnsForAnyArgs(authResponse);

        // Act
        var result = await _authController.RefreshAsync(refreshRequest);

        // Assert
        _mapper.Received().Map<RefreshCommand>(refreshRequest);
        await _mediator.Received().Send(refreshCommand);
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(authResponse);
    }

    [Test, AutoData]
    public async Task Logout_ShouldReturnCorrectResult(Guid userId, Guid clientId)
    {
        // Arrange
        LogoutCommand capturedLogoutCommand = null!;

        _authController.ControllerContext.HttpContext = CreateHttpContext(userId, clientId);

        _mediator
            .WhenForAnyArgs(x => x.Send((LogoutCommand)default!))
            .Do(x => capturedLogoutCommand = x.Arg<LogoutCommand>());

        // Act
        var result = await _authController.LogoutAsync();

        // Assert
        await _mediator.Received().Send(capturedLogoutCommand);
        result.Should().BeOfType<OkResult>();
        capturedLogoutCommand.UserId.Should().Be(userId);
        capturedLogoutCommand.ClientId.Should().Be(clientId);
    }
}