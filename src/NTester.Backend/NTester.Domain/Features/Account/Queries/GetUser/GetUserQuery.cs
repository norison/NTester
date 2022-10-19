using MediatR;
using NTester.DataContracts.Account.GetUser;

namespace NTester.Domain.Features.Account.Queries.GetUser;

/// <summary>
/// Query to get the user.
/// </summary>
public class GetUserQuery : IRequest<GetUserResponse>
{
    /// <summary>
    /// Id of the user.
    /// </summary>
    public Guid UserId { get; set; }
}