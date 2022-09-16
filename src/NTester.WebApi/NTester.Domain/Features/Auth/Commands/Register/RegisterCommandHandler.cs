using MediatR;
using NTester.DataAccess.Entities;
using NTester.DataAccess.Services.Transaction;
using NTester.DataContracts.Auth;
using NTester.Domain.Exceptions.Auth;
using NTester.Domain.Exceptions.Common;
using NTester.Domain.Services.Auth;
using NTester.Domain.Services.Cookie;
using NTester.Domain.Services.UserManager;

namespace NTester.Domain.Features.Auth.Commands.Register;

/// <summary>
/// Handler of the registration command.
/// </summary>
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IUserManager _userManager;
    private readonly IAuthService _authService;
    private readonly ICookieService _cookieService;
    private readonly ITransactionFactory _transactionFactory;

    /// <summary>
    /// Creates an instance of the registration command handler.
    /// </summary>
    /// <param name="userManager">Manager of the users.</param>
    /// <param name="authService">Service of the authentication.</param>
    /// <param name="cookieService">Service for the cookies.</param>
    /// <param name="transactionFactory">Factory of the transactions.</param>
    public RegisterCommandHandler(
        IUserManager userManager,
        IAuthService authService,
        ICookieService cookieService,
        ITransactionFactory transactionFactory)
    {
        _userManager = userManager;
        _authService = authService;
        _cookieService = cookieService;
        _transactionFactory = transactionFactory;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle"/>
    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        await ValidateIfUserExists(request.UserName);

        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            Email = request.Email,
            Name = request.Name,
            Surname = request.Surname
        };

        await using var transaction = await _transactionFactory.CreateTransactionAsync(cancellationToken);

        await CreateUserAsync(user, request.Password);

        var result = await _authService.AuthenticateUserAsync(user, request.ClientId);

        _cookieService.SetRefreshToken(result.RefreshToken);

        await transaction.CommitAsync(cancellationToken);

        return result;
    }

    private async Task ValidateIfUserExists(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user != null)
        {
            throw new InvalidUserNameOrPasswordException();
        }
    }

    private async Task CreateUserAsync(UserEntity user, string password)
    {
        var identityResult = await _userManager.CreateAsync(user, password);

        if (!identityResult.Succeeded)
        {
            throw new NonGeneralException(identityResult.Errors.First().Description);
        }
    }
}