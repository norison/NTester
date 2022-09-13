using System.Net;
using MediatR;
using NTester.DataAccess.Entities;
using NTester.DataAccess.Services.Transaction;
using NTester.DataContracts.Auth;
using NTester.Domain.Exceptions;
using NTester.Domain.Services.Auth;
using NTester.Domain.Services.SignInManager;
using NTester.Domain.Services.UserManager;

namespace NTester.Domain.Features.Auth.Commands.Login;

/// <summary>
/// Handler of the login command.
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUserManager _userManager;
    private readonly ISignInManager _signInManager;
    private readonly IAuthService _authService;

    /// <summary>
    /// Creates an instance of the login command handler.
    /// </summary>
    /// <param name="userManager">Manager of the users.</param>
    /// <param name="signInManager">Manager of the sign in.</param>
    /// <param name="authService">Service for the authentication.</param>
    public LoginCommandHandler(IUserManager userManager, ISignInManager signInManager, IAuthService authService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle"/>
    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        await ValidateUser(user, request.Password);

        return await _authService.AuthenticateUserAsync(user, request.ClientId, cancellationToken);
    }

    private async Task ValidateUser(UserEntity user, string password)
    {
        if (user == null)
        {
            throw new RestException(HttpStatusCode.NotFound, "User was not found.");
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, false);

        if (!signInResult.Succeeded)
        {
            throw new RestException(HttpStatusCode.BadRequest, "User was not found.");
        }
    }
}