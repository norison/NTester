using NTester.DataContracts.Common;

namespace NTester.DataContracts.Tests.GetTests.Base;

/// <summary>
/// Base request to retrieve the tests.
/// </summary>
public class GetTestsRequestBase : PageRequest
{
    /// <summary>
    /// Test title must partially match the property value.
    /// </summary>
    public string Title { get; set; } = string.Empty;
}