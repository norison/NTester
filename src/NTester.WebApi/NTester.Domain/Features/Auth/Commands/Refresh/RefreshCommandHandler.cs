using MediatR;
using NTester.DataContracts.Auth;
using NTester.Domain.Exceptions;
using NTester.Domain.Exceptions.Codes;
using NTester.Domain.Services.Auth;
using NTester.Domain.Services.Cookie;

namespace NTester.Domain.Features.Auth.Commands.Refresh;

/// <summary>
/// Handler of the refresh command.
/// </summary>
public class RefreshCommandHandler : IRequestHandler<RefreshCommand, AuthResponse>
{
    private readonly IAuthService _authService;
    private readonly ICookieService _cookieService;

    private const string ErrorMessageRefreshTokenNotProvided = "Refresh token was not provided.";

    /// <summary>
    /// Creates an instance of the refresh command handler.
    /// </summary>
    /// <param name="authService">Service for the authentication.</param>
    /// <param name="cookieService">Service for the cookies.</param>
    public RefreshCommandHandler(IAuthService authService, ICookieService cookieService)
    {
        _authService = authService;
        _cookieService = cookieService;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle"/>
    public async Task<AuthResponse> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = _cookieService.GetRefreshToken();

        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new ValidationException((int)AuthCodes.RefreshTokenWasNotProvided,
                ErrorMessageRefreshTokenNotProvided);
        }

        var result = await _authService.AuthenticateUserAsync(request.AccessToken, refreshToken);

        _cookieService.SetRefreshToken(result.RefreshToken);

        return result;
    }
}