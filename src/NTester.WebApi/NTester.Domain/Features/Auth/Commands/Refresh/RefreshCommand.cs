using MediatR;
using NTester.DataContracts.Auth;

namespace NTester.Domain.Features.Auth.Commands.Refresh;

/// <summary>
/// Command to get a new pair of tokens.
/// </summary>
public class RefreshCommand : IRequest<AuthResponse>
{
    /// <summary>
    /// Token for accessing the resource.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;
}