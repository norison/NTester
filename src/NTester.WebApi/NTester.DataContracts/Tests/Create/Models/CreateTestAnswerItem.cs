namespace NTester.DataContracts.Tests.Create.Models;

/// <summary>
/// Answer request item to create a test.
/// </summary>
public class CreateTestAnswerItem
{
    /// <summary>
    /// Content of the answer.
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    /// Represents the correct answer or not.
    /// </summary>
    public bool IsCorrect { get; set; }
}