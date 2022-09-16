using MediatR;
using NTester.DataAccess.Entities;
using NTester.DataContracts.Auth;
using NTester.Domain.Exceptions;
using NTester.Domain.Exceptions.Codes;
using NTester.Domain.Services.Auth;
using NTester.Domain.Services.Cookie;
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
    private readonly ICookieService _cookieService;

    private const string ErrorMessageIncorrectUserNameOrPassword = "Provided incorrect user name or password";

    /// <summary>
    /// Creates an instance of the login command handler.
    /// </summary>
    /// <param name="userManager">Manager of the users.</param>
    /// <param name="signInManager">Manager of the sign in.</param>
    /// <param name="authService">Service for the authentication.</param>
    /// <param name="cookieService">Service for the cookies.</param>
    public LoginCommandHandler(
        IUserManager userManager,
        ISignInManager signInManager,
        IAuthService authService,
        ICookieService cookieService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
        _cookieService = cookieService;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle"/>
    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        await ValidateUser(user, request.Password);

        var result = await _authService.AuthenticateUserAsync(user, request.ClientId);

        _cookieService.SetRefreshToken(result.RefreshToken);

        return result;
    }

    private async Task ValidateUser(UserEntity user, string password)
    {
        if (user == null)
        {
            throw new ValidationException((int)AuthCodes.IncorrectUserNameOrPassword,
                ErrorMessageIncorrectUserNameOrPassword);
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, false);

        if (!signInResult.Succeeded)
        {
            throw new ValidationException((int)AuthCodes.IncorrectUserNameOrPassword,
                ErrorMessageIncorrectUserNameOrPassword);
        }
    }
}