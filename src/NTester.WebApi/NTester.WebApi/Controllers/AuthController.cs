using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTester.DataContracts;
using NTester.DataContracts.Auth;
using NTester.DataContracts.Auth.Login;
using NTester.DataContracts.Auth.Refresh;
using NTester.DataContracts.Auth.Register;
using NTester.Domain.Extensions;
using NTester.Domain.Features.Auth.Commands.Login;
using NTester.Domain.Features.Auth.Commands.Logout;
using NTester.Domain.Features.Auth.Commands.Refresh;
using NTester.Domain.Features.Auth.Commands.Register;

namespace NTester.WebApi.Controllers;

/// <summary>
/// Manages authentication operations.
/// </summary>
[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of the authentication controller.
    /// </summary>
    /// <param name="mediator">Mediator.</param>
    /// <param name="mapper">Mapper.</param>
    public AuthController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Logins a user.
    /// </summary>
    /// <param name="loginRequest">Request for the login.</param>
    /// <returns>Authentication response.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">If invalid data provided.</response>
    /// <response code="500">If server error occured.</response>
    [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginRequest loginRequest)
    {
        var command = _mapper.Map<LoginCommand>(loginRequest);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Registers a user.
    /// </summary>
    /// <param name="registerRequest">Request for the registration.</param>
    /// <returns>Authentication response.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">If invalid data provided.</response>
    /// <response code="500">If server error occured.</response>
    [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest registerRequest)
    {
        var command = _mapper.Map<RegisterCommand>(registerRequest);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Refreshes the token pair.
    /// </summary>
    /// <param name="refreshRequest">Request for the refresh.</param>
    /// <returns>Authentication response.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">If invalid data provided.</response>
    /// <response code="500">If server error occured.</response>
    [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync(RefreshRequest refreshRequest)
    {
        var command = _mapper.Map<RefreshCommand>(refreshRequest);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Logouts a user.
    /// </summary>
    /// <response code="200">Success.</response>
    /// <response code="400">If invalid data provided.</response>
    /// <response code="401">If user is unauthorized.</response>
    /// <response code="500">If server error occured.</response>
    [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> LogoutAsync()
    {
        var command = new LogoutCommand
        {
            ClientId = HttpContext.User.GetClientId(),
            UserId = HttpContext.User.GetUserId()
        };

        await _mediator.Send(command);
        return Ok();
    }
}