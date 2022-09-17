using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NTester.Domain.Constants;

namespace NTester.WebApi.Tests.Controllers.Base;

public abstract class ControllerTestsBase
{
    protected static HttpContext CreateHttpContext(Guid userId = default, Guid clientId = default)
    {
        return new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimConstants.UserIdClaimTypeName, userId.ToString()),
                new(ClaimConstants.ClientIdClaimTypeName, clientId.ToString())
            }))
        };
    }
}