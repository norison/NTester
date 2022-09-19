using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NTester.Domain.Constants;
using NTester.WebApi.Constants;

namespace NTester.WebApi.Tests.Controllers.Base;

public abstract class ControllerTestsBase
{
    protected static HttpContext CreateHttpContext(Guid userId = default, string clientName = "client")
    {
        var context = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimConstants.UserIdClaimTypeName, userId.ToString()),
                new(ClaimConstants.ClientNameClaimTypeName, clientName)
            }))
        };
        
        context.Request.Headers.Add(HeaderConstants.Client, clientName);

        return context;
    }
}