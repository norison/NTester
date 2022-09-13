using Microsoft.AspNetCore.Mvc;

namespace NTester.WebApi.Controllers.Base;

/// <summary>
/// Base controller for the application.
/// </summary>
public abstract class ApiControllerBase : ControllerBase
{
    private Guid _userId;
    private Guid _clientId;

    /// <summary>
    /// Id of the user.
    /// </summary>
    protected Guid UserId
    {
        get
        {
            if (_userId == Guid.Empty)
            {
                _userId = HttpContext.User.Identity is { IsAuthenticated: true }
                    ? Guid.Parse(HttpContext.User.Claims.First(x => x.Type == "id").Value)
                    : Guid.Empty;
            }

            return _userId;
        }
    }

    /// <summary>
    /// Id of client user.
    /// </summary>
    protected Guid ClientId
    {
        get
        {
            if (_clientId == Guid.Empty)
            {
                _clientId = HttpContext.User.Identity is { IsAuthenticated: true }
                    ? Guid.Parse(HttpContext.User.Claims.First(x => x.Type == "client").Value)
                    : Guid.Empty;
            }

            return _clientId;
        }
    }
}