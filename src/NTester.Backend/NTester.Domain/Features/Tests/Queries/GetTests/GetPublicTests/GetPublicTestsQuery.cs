using MediatR;
using NTester.DataContracts.Tests.GetTests;

namespace NTester.Domain.Features.Tests.Queries.GetTests.GetPublicTests;

/// <summary>
/// Query to retrieve the public tests.
/// </summary>
public class GetPublicTestsQuery : GetTestsQueryBase, IRequest<GetTestsResponse>
{
}