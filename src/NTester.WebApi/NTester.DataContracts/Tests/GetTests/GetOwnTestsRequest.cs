using NTester.DataContracts.Tests.GetTests.Base;

namespace NTester.DataContracts.Tests.GetTests;

/// <summary>
/// Request to retrieve the own tests.
/// </summary>
public class GetOwnTestsRequest : GetTestsRequestBase
{
    /// <summary>
    /// Whether published or not.
    /// </summary>
    public bool Published { get; set; }
}