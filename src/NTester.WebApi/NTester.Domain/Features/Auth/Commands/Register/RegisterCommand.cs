using MediatR;
using NTester.DataContracts.Auth;

namespace NTester.Domain.Features.Auth.Commands.Register;

/// <summary>
/// Command for user registration.
/// </summary>
public class RegisterCommand : IRequest<AuthResponse>
{
    /// <summary>
    /// User name of the user.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Email of the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Password of the user.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Name of the user.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Surname of the user.
    /// </summary>
    public string Surname { get; set; } = string.Empty;

    /// <summary>
    /// Name of the client.
    /// </summary>
    public string ClientName { get; set; } = string.Empty;
}