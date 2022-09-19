using NTester.DataContracts.Tests.GetTests.Models;

namespace NTester.DataContracts.Tests.GetTests;

/// <summary>
/// Response on <see cref="GetOwnTestsRequest"/>.
/// </summary>
public class GetTestsResponse
{
    /// <summary>
    /// Total count of the tests.
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// Collection of the tests.
    /// </summary>
    public IEnumerable<GetTestsResponseItem> Tests { get; set; } = Array.Empty<GetTestsResponseItem>();
}