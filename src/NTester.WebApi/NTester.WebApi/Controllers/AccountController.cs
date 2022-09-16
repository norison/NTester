using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTester.DataContracts;
using NTester.DataContracts.Account.GetUser;
using NTester.Domain.Extensions;
using NTester.Domain.Features.Account.GetUser;

namespace NTester.WebApi.Controllers;

/// <summary>
/// Manages account operations.
/// </summary>
[Produces("application/json")]
[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Creates an instance of the <see cref="AccountController"/>.
    /// </summary>
    /// <param name="mediator">Mediator.</param>
    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets the information of the authenticated user.
    /// </summary>
    /// <returns>Information of the authenticated user.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">If invalid data provided.</response>
    /// <response code="401">If user is unauthorized.</response>
    /// <response code="404">If user not found.</response>
    /// <response code="500">If server error occured.</response>
    [ProducesResponseType(typeof(GetUserResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [HttpGet]
    public async Task<IActionResult> GetUserAsync()
    {
        var getUserCommand = new GetUserCommand { UserId = HttpContext.User.GetUserId() };
        var result = await _mediator.Send(getUserCommand);
        return Ok(result);
    }
}