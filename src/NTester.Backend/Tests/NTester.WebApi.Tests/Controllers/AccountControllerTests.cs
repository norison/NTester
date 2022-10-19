using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NTester.DataContracts.Account.GetUser;
using NTester.Domain.Features.Account.Queries.GetUser;
using NTester.WebApi.Controllers;
using NTester.WebApi.Tests.Controllers.Base;
using NUnit.Framework;

namespace NTester.WebApi.Tests.Controllers;

[TestFixture]
public class AccountControllerTests : ControllerTestsBase
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
        GetUserQuery capturedQuery = null!;

        _accountController.ControllerContext.HttpContext = CreateHttpContext(userId);

        _mediator
            .Send((GetUserQuery)default!)
            .ReturnsForAnyArgs(getUserResponse)
            .AndDoes(x => capturedQuery = x.Arg<GetUserQuery>());

        // Act
        var result = await _accountController.GetUserAsync();

        // Assert
        await _mediator.Received().Send(capturedQuery);

        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(getUserResponse);
        capturedQuery.UserId.Should().Be(userId);
    }
}