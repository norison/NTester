using MediatR;
using NTester.DataContracts.Tests.GetTestById;

namespace NTester.Domain.Features.Tests.Queries.GetTestById;

/// <summary>
/// Query to retrieve a test by id.
/// </summary>
public class GetTestByIdQuery : IRequest<GetTestByIdResponse>
{
    /// <summary>
    /// Id of the user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Id of the test.
    /// </summary>
    public Guid Id { get; set; }
}