using AutoFixture.NUnit3;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NTester.DataContracts.Tests.Create;
using NTester.DataContracts.Tests.GetTests;
using NTester.Domain.Features.Tests.Commands.Create;
using NTester.Domain.Features.Tests.Queries.GetTests.GetOwnTests;
using NTester.Domain.Features.Tests.Queries.GetTests.GetPublicTests;
using NTester.WebApi.Controllers;
using NTester.WebApi.Tests.Controllers.Base;
using NUnit.Framework;

namespace NTester.WebApi.Tests.Controllers;

[TestFixture]
public class TestsControllerTests : ControllerTestsBase
{
    private IMediator _mediator;
    private IMapper _mapper;
    private TestsController _testsController;

    [SetUp]
    public void SetUp()
    {
        _mediator = Substitute.For<IMediator>();
        _mapper = Substitute.For<IMapper>();

        _testsController = new TestsController(_mediator, _mapper);
    }

    [Test, AutoData]
    public async Task CreateTestAsync_ShouldReturnCorrectResult(
        CreateTestRequest createTestRequest,
        CreateTestCommand createTestCommand,
        CreateTestResponse createTestResponse,
        Guid userId)
    {
        // Arrange
        _mapper.Map<CreateTestCommand>(default).ReturnsForAnyArgs(createTestCommand);
        _mediator.Send(createTestCommand).ReturnsForAnyArgs(createTestResponse);
        _testsController.ControllerContext.HttpContext = CreateHttpContext(userId);

        // Act
        var result = await _testsController.CreateTestAsync(createTestRequest);

        // Assert
        _mapper.Received().Map<CreateTestCommand>(createTestRequest);
        await _mediator.Received().Send(createTestCommand);
        createTestCommand.UserId.Should().Be(userId);
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(createTestResponse);
    }

    [Test, AutoData]
    public async Task GetOwnTestsAsync_ShouldReturnCorrectResult(
        GetOwnTestsRequest getOwnTestsRequest,
        GetOwnTestsQuery getOwnTestsQuery,
        GetTestsResponse getTestsResponse,
        Guid userId)
    {
        // Arrange
        _mapper.Map<GetOwnTestsQuery>(default).ReturnsForAnyArgs(getOwnTestsQuery);
        _mediator.Send(getOwnTestsQuery).ReturnsForAnyArgs(getTestsResponse);
        _testsController.ControllerContext.HttpContext = CreateHttpContext(userId);

        // Act
        var result = await _testsController.GetOwnTestsAsync(getOwnTestsRequest);

        // Assert
        _mapper.Received().Map<GetOwnTestsQuery>(getOwnTestsRequest);
        await _mediator.Received().Send(getOwnTestsQuery);
        getOwnTestsQuery.UserId.Should().Be(userId);
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(getTestsResponse);
    }
    
    [Test, AutoData]
    public async Task GetPublicTestsAsync_ShouldReturnCorrectResult(
        GetPublicTestsRequest getPublicTestsRequest,
        GetPublicTestsQuery getPublicTestsQuery,
        GetTestsResponse getTestsResponse,
        Guid userId)
    {
        // Arrange
        _mapper.Map<GetPublicTestsQuery>(default).ReturnsForAnyArgs(getPublicTestsQuery);
        _mediator.Send(getPublicTestsQuery).ReturnsForAnyArgs(getTestsResponse);
        _testsController.ControllerContext.HttpContext = CreateHttpContext(userId);

        // Act
        var result = await _testsController.GetPublicTestsAsync(getPublicTestsRequest);

        // Assert
        _mapper.Received().Map<GetPublicTestsQuery>(getPublicTestsRequest);
        await _mediator.Received().Send(getPublicTestsQuery);
        getPublicTestsQuery.UserId.Should().Be(userId);
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().Be(getTestsResponse);
    }
}