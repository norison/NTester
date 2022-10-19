using MediatR;
using NTester.DataContracts.Tests.GetTests;

namespace NTester.Domain.Features.Tests.Queries.GetTests.GetOwnTests;

/// <summary>
/// Query to retrieve the own tests.
/// </summary>
public class GetOwnTestsQuery : GetTestsQueryBase, IRequest<GetTestsResponse>
{
    /// <summary>
    /// Whether published or not.
    /// </summary>
    public bool Published { get; set; }
}