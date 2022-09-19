namespace NTester.Domain.Features.Tests.Queries.Common;

/// <summary>
/// Request fot page.
/// </summary>
public class PageQuery
{
    /// <summary>
    /// Number of the page.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Size of the page.
    /// </summary>
    public int PageSize { get; set; }
}