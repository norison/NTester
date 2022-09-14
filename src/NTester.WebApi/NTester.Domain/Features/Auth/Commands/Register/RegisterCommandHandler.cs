using System.Net;
using MediatR;
using NTester.DataAccess.Entities;
using NTester.DataAccess.Services.Transaction;
using NTester.DataContracts.Auth;
using NTester.Domain.Exceptions;
using NTester.Domain.Services.Auth;
using NTester.Domain.Services.Cookie;
using NTester.Domain.Services.SignInManager;
using NTester.Domain.Services.UserManager;

namespace NTester.Domain.Features.Auth.Commands.Register;

/// <summary>
/// Handler of the registration command.
/// </summary>
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IUserManager _userManager;
    private readonly ISignInManager _signInManager;
    private readonly IAuthService _authService;
    private readonly ICookieService _cookieService;
    private readonly ITransactionFactory _transactionFactory;

    /// <summary>
    /// Creates an instance of the registration command handler.
    /// </summary>
    /// <param name="userManager">Manager of the users.</param>
    /// <param name="signInManager">Manager of the sign in.</param>
    /// <param name="authService">Service of the authentication.</param>
    /// <param name="cookieService">Service for the cookies.</param>
    /// <param name="transactionFactory">Factory of the transactions.</param>
    public RegisterCommandHandler(
        IUserManager userManager,
        ISignInManager signInManager,
        IAuthService authService,
        ICookieService cookieService,
        ITransactionFactory transactionFactory)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
        _cookieService = cookieService;
        _transactionFactory = transactionFactory;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle"/>
    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        await ValidateIfUserExists(request);

        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            Email = request.Email,
            Name = request.Name,
            Surname = request.Surname
        };

        await using var transaction = await _transactionFactory.CreateTransactionAsync(cancellationToken);

        try
        {
            await CreateUserAsync(user, request.Password);

            var result = await _authService.AuthenticateUserAsync(user, request.ClientId);

            _cookieService.SetRefreshToken(result.RefreshToken);

            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task ValidateIfUserExists(RegisterCommand request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user != null)
        {
            throw new RestException(HttpStatusCode.BadRequest, "User with the same user name already exists.");
        }
    }

    private async Task CreateUserAsync(UserEntity user, string password)
    {
        var identityResult = await _userManager.CreateAsync(user, password);

        if (!identityResult.Succeeded)
        {
            throw new RestException(HttpStatusCode.BadRequest, identityResult.Errors.First().Description);
        }
    }
}