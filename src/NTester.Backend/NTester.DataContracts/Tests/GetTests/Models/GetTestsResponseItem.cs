namespace NTester.DataContracts.Tests.GetTests.Models;

/// <summary>
/// Get tests response item.
/// </summary>
public class GetTestsResponseItem
{
    /// <summary>
    /// Id of the user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Id of the test.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Title of the test.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description of the test.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Date and time of the creation.
    /// </summary>
    public DateTime CreationDateTime { get; set; }

    /// <summary>
    /// Whether published or not.
    /// </summary>
    public bool Published { get; set; }
}