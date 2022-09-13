using System.Security.Claims;

namespace NTester.Domain.Extensions;

/// <summary>
/// Extensions for the claim principal.
/// </summary>
public static class ClaimPrincipalExtensions
{
    private const string UserIdClaimType = "id";
    private const string ClientIdClaimType = "client";

    /// <summary>
    /// Gets user id from the claim principal.
    /// </summary>
    /// <param name="claimsPrincipal">Claim principal.</param>
    /// <returns>Id of the user.</returns>
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        return Guid.Parse(claimsPrincipal.Claims.First(x => x.Type == UserIdClaimType).Value);
    }

    /// <summary>
    /// Gets client id from the claim principal.
    /// </summary>
    /// <param name="claimsPrincipal">Claim principal.</param>
    /// <returns>Id of the client.</returns>
    public static Guid GetClientId(this ClaimsPrincipal claimsPrincipal)
    {
        return Guid.Parse(claimsPrincipal.Claims.First(x => x.Type == ClientIdClaimType).Value);
    }
}