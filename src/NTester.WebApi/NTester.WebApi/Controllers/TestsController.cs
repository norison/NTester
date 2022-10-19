using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTester.DataContracts;
using NTester.DataContracts.Tests.Create;
using NTester.DataContracts.Tests.GetTestById;
using NTester.DataContracts.Tests.GetTests;
using NTester.Domain.Extensions;
using NTester.Domain.Features.Tests.Commands.Create;
using NTester.Domain.Features.Tests.Queries.GetTestById;
using NTester.Domain.Features.Tests.Queries.GetTests.GetOwnTests;
using NTester.Domain.Features.Tests.Queries.GetTests.GetPublicTests;

namespace NTester.WebApi.Controllers;

/// <summary>
/// Manages operations with tests.
/// </summary>
[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class TestsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of the <see cref="TestsController"/>.
    /// </summary>
    /// <param name="mediator">Mediator.</param>
    /// <param name="mapper">Mapper.</param>
    public TestsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a collection of the own tests.
    /// </summary>
    /// <param name="getPublicTestsRequest">Request to retrieve the own tests.</param>
    /// <returns>Collection of the tests.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">If invalid data provided.</response>
    /// <response code="401">If user is unauthorized.</response>
    /// <response code="500">If server error occurred.</response>
    [ProducesResponseType(typeof(GetTestsResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [HttpGet]
    public async Task<IActionResult> GetOwnTestsAsync([FromQuery] GetOwnTestsRequest getPublicTestsRequest)
    {
        var query = _mapper.Map<GetOwnTestsQuery>(getPublicTestsRequest);
        query.UserId = User.GetUserId();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets a collection of the public tests.
    /// </summary>
    /// <param name="getPublicTestsRequest">Request to retrieve the public tests.</param>
    /// <returns>Collection of the tests.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">If invalid data provided.</response>
    /// <response code="500">If server error occurred.</response>
    [ProducesResponseType(typeof(GetTestsResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [HttpGet("public")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublicTestsAsync([FromQuery] GetPublicTestsRequest getPublicTestsRequest)
    {
        var query = _mapper.Map<GetPublicTestsQuery>(getPublicTestsRequest);
        query.UserId = User.GetUserId();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets a test by id.
    /// </summary>
    /// <param name="id">Id of the test.</param>
    /// <returns>Information about the test.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">If invalid data provided.</response>
    /// <response code="404">If test was not found.</response>
    /// <response code="500">If server error occurred.</response>
    [ProducesResponseType(typeof(GetTestByIdResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTestByIdAsync(Guid id)
    {
        var getTestByIdQuery = new GetTestByIdQuery { Id = id, UserId = User.GetUserId() };
        var result = await _mediator.Send(getTestByIdQuery);
        return Ok(result);
    }

    /// <summary>
    /// Creates a test.
    /// </summary>
    /// <param name="createTestRequest">Request to create a test.</param>
    /// <returns>Response on test creation.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">If invalid data provided.</response>
    /// <response code="401">If user is unauthorized.</response>
    /// <response code="500">If server error occurred.</response>
    [ProducesResponseType(typeof(CreateTestResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [HttpPost("create")]
    public async Task<IActionResult> CreateTestAsync(CreateTestRequest createTestRequest)
    {
        var command = _mapper.Map<CreateTestCommand>(createTestRequest);
        command.UserId = User.GetUserId();
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}