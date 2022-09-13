using MediatR;

namespace NTester.Domain.Features.Auth.Commands.Logout;

/// <summary>
/// Command for user logout.
/// </summary>
public class LogoutCommand : IRequest<Unit>
{
    /// <summary>
    /// Id of the client.
    /// </summary>
    public Guid ClientId { get; set; }

    /// <summary>
    /// Id of the user.
    /// </summary>
    public Guid UserId { get; set; }
}