using System.Security.Claims;
using NTester.Domain.Constants;

namespace NTester.Domain.Extensions;

/// <summary>
/// Extensions for the claim principal.
/// </summary>
public static class ClaimPrincipalExtensions
{
    /// <summary>
    /// Gets user id from the claim principal.
    /// </summary>
    /// <param name="claimsPrincipal">Claim principal.</param>
    /// <returns>Id of the user.</returns>
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        return GetGuidValue(claimsPrincipal, ClaimConstants.UserIdClaimTypeName);
    }

    /// <summary>
    /// Gets client id from the claim principal.
    /// </summary>
    /// <param name="claimsPrincipal">Claim principal.</param>
    /// <returns>Id of the client.</returns>
    public static Guid GetClientId(this ClaimsPrincipal claimsPrincipal)
    {
        return GetGuidValue(claimsPrincipal, ClaimConstants.ClientIdClaimTypeName);
    }

    private static Guid GetGuidValue(ClaimsPrincipal claimsPrincipal, string claimType)
    {
        var value = claimsPrincipal.FindFirstValue(claimType);
        return value == null ? Guid.Empty : Guid.Parse(value);
    }
}