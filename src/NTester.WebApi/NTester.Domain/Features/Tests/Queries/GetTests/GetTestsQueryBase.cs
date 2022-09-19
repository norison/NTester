using NTester.Domain.Features.Tests.Queries.Common;

namespace NTester.Domain.Features.Tests.Queries.GetTests;

/// <summary>
/// Base query to retrieve the tests.
/// </summary>
public abstract class GetTestsQueryBase : PageQuery
{
    /// <summary>
    /// Id of the user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Test title must partially match the property value.
    /// </summary>
    public string Title { get; set; } = string.Empty;
}