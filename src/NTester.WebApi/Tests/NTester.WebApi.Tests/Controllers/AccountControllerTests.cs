using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NTester.DataContracts.Account.GetUser;
using NTester.Domain.Constants;
using NTester.Domain.Features.Account.GetUser;
using NTester.WebApi.Controllers;
using NUnit.Framework;

namespace NTester.WebApi.Tests.Controllers;

[TestFixture]
public class AccountControllerTests
{
    private IMediator _mediator;
    private AccountController _accountController;

    [SetUp]
    public void SetUp()
    {
        _mediator = Substitute.For<IMediator>();
        _accountController = new AccountController(_mediator);
    }

    [Test, AutoData]
    public async Task GetUserAsync_ShouldReturnCorrectResult(GetUserResponse getUserResponse, Guid userId)
    {
        // Arrange
        GetUserCommand capturedCommand = null!;

        _accountController.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimConstants.UserIdClaimTypeName, userId.ToString())
            }))
        };

        _mediator
            .Send((GetUserCommand)default!)
            .ReturnsForAnyArgs(getUserResponse)
            .AndDoes(x => capturedCommand = x.Arg<GetUserCommand>());

        // Act
        var result = await _accountController.GetUserAsync();

        // Assert
        await _mediator.Received().Send(capturedCommand);

        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(getUserResponse);
        capturedCommand.UserId.Should().Be(userId);
    }
}