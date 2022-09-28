namespace NTester.DataContracts.Tests.GetTestById.Models;

/// <summary>
/// Answer response item.
/// </summary>
public class GetTestByIdAnswerItem
{
    /// <summary>
    /// Id of the answer.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Content of the answer.
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    /// Represents the correct answer or not.
    /// </summary>
    public bool IsCorrect { get; set; }
}