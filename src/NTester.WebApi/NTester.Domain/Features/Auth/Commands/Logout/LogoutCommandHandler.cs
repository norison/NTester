using MediatR;
using NTester.Domain.Services.Auth;
using NTester.Domain.Services.Cookie;

namespace NTester.Domain.Features.Auth.Commands.Logout;

/// <summary>
/// Handler of the logout command.
/// </summary>
public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IAuthService _authService;
    private readonly ICookieService _cookieService;

    /// <summary>
    /// Creates an instance of the logout command handler.
    /// </summary>
    /// <param name="authService">Service for the authentication.</param>
    /// <param name="cookieService">Service for the cookies.</param>
    public LogoutCommandHandler(IAuthService authService, ICookieService cookieService)
    {
        _authService = authService;
        _cookieService = cookieService;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle"/>
    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = _cookieService.GetRefreshToken();
        _cookieService.RemoveRefreshToken();

        await _authService.RevokeRefreshTokenAsync(refreshToken, request.ClientId, request.UserId);

        return Unit.Value;
    }
}