using System.Security.Claims;

namespace NTester.Domain.Extensions;

/// <summary>
/// Extensions for the claim principal.
/// </summary>
public static class ClaimPrincipalExtensions
{
    /// <summary>
    /// Claim type name of the user id.
    /// </summary>
    public const string UserIdClaimType = "id";

    /// <summary>
    /// Claim type name of the client id.
    /// </summary>
    public const string ClientIdClaimType = "client";

    /// <summary>
    /// Gets user id from the claim principal.
    /// </summary>
    /// <param name="claimsPrincipal">Claim principal.</param>
    /// <returns>Id of the user.</returns>
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        return GetGuidValue(claimsPrincipal, UserIdClaimType);
    }

    /// <summary>
    /// Gets client id from the claim principal.
    /// </summary>
    /// <param name="claimsPrincipal">Claim principal.</param>
    /// <returns>Id of the client.</returns>
    public static Guid GetClientId(this ClaimsPrincipal claimsPrincipal)
    {
        return GetGuidValue(claimsPrincipal, ClientIdClaimType);
    }

    private static Guid GetGuidValue(ClaimsPrincipal claimsPrincipal, string claimType)
    {
        var value = claimsPrincipal.FindFirstValue(claimType);
        return value == null ? Guid.Empty : Guid.Parse(value);
    }
}