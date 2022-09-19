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
        var value = claimsPrincipal.FindFirstValue(ClaimConstants.UserIdClaimTypeName);
        return value == null ? Guid.Empty : Guid.Parse(value);
    }

    /// <summary>
    /// Gets client id from the claim principal.
    /// </summary>
    /// <param name="claimsPrincipal">Claim principal.</param>
    /// <returns>Id of the client.</returns>
    public static string GetClientName(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(ClaimConstants.ClientNameClaimTypeName);
    }
}