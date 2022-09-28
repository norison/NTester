using NTester.DataContracts.Tests.GetTestById.Models;

namespace NTester.DataContracts.Tests.GetTestById;

/// <summary>
/// 
/// </summary>
public class GetTestByIdResponse
{
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
    /// If test is published or not.
    /// </summary>
    public bool Published { get; set; }

    /// <summary>
    /// Questions of the test.
    /// </summary>
    public IEnumerable<GetTestByIdQuestionItem> Questions { get; set; } = Array.Empty<GetTestByIdQuestionItem>();
}