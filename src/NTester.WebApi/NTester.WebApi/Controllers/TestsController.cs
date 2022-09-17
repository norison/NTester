using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTester.DataContracts;
using NTester.DataContracts.Tests.Create;
using NTester.Domain.Extensions;
using NTester.Domain.Features.Tests.Commands.Create;

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
    /// Creates a test.
    /// </summary>
    /// <param name="createTestRequest">Request to create a test.</param>
    /// <returns>Response on test creation.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">If invalid data provided.</response>
    /// <response code="401">If user is unauthorized.</response>
    /// <response code="500">If server error occured.</response>
    [ProducesResponseType(typeof(CreateTestResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [HttpPost("create")]
    public async Task<IActionResult> CreateTestAsync(CreateTestRequest createTestRequest)
    {
        var command = _mapper.Map<CreateTestCommand>(createTestRequest);
        command.UserId = HttpContext.User.GetUserId();
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}