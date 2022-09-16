using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
[ApiController]
[Route("api/[controller]")]
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
    [HttpPost("refresh")]
    public async Task<IActionResult> RegisterAsync(RefreshRequest refreshRequest)
    {
        var command = _mapper.Map<RefreshCommand>(refreshRequest);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Logouts a user.
    /// </summary>
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