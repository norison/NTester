using System.Net;
using System.Security.Claims;
using MediatR;
using Microsoft.IdentityModel.JsonWebTokens;
using NTester.DataAccess.Data.NTesterDbContext;
using NTester.DataAccess.Entities;
using NTester.DataContracts.Auth;
using NTester.Domain.Exceptions;
using NTester.Domain.Services.SignInManager;
using NTester.Domain.Services.Token;
using NTester.Domain.Services.UserManager;

namespace NTester.Domain.Features.Commands.Login;

/// <summary>
/// Handler of the login command.
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUserManager _userManager;
    private readonly ISignInManager _signInManager;
    private readonly INTesterDbContext _dbContext;
    private readonly ITokenService _tokenService;

    /// <summary>
    /// Creates an instance of the login command handler.
    /// </summary>
    /// <param name="userManager">Manager of the users.</param>
    /// <param name="signInManager">Manager of the sign in.</param>
    /// <param name="dbContext">Database context of the application.</param>
    /// <param name="tokenService">Token service.</param>
    public LoginCommandHandler(
        IUserManager userManager,
        ISignInManager signInManager,
        INTesterDbContext dbContext,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _dbContext = dbContext;
        _tokenService = tokenService;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle"/>
    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        await ValidateUser(request, user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, user.Id.ToString())
        };

        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshTokenEntity
        {
            Token = refreshToken,
            ExpirationDateTime = DateTime.UtcNow.AddMonths(6),
            UserId = user.Id,
        };

        await _dbContext.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    private async Task ValidateUser(LoginCommand request, UserEntity user)
    {
        if (user == null)
        {
            throw new RestException(HttpStatusCode.NotFound, "User was not found.");
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!signInResult.Succeeded)
        {
            throw new RestException(HttpStatusCode.BadRequest, "User was not found.");
        }
    }
}