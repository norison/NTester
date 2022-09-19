namespace NTester.DataContracts.Common;

/// <summary>
/// Request for the page.
/// </summary>
public class PageRequest
{
    /// <summary>
    /// Number of the page.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Size of the page.
    /// </summary>
    public int PageSize { get; set; } = 5;
}