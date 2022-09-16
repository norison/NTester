using MediatR;
using NTester.DataContracts.Account.GetUser;

namespace NTester.Domain.Features.Account.GetUser;

/// <summary>
/// Command to get the user.
/// </summary>
public class GetUserCommand : IRequest<GetUserResponse>
{
    /// <summary>
    /// Id of the user.
    /// </summary>
    public Guid UserId { get; set; }
}