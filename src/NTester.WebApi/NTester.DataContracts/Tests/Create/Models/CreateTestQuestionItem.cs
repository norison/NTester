namespace NTester.DataContracts.Tests.Create.Models;

/// <summary>
/// Question request item to create a test.
/// </summary>
public class CreateTestQuestionItem
{
    /// <summary>
    /// Content of the question.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Answers of the question.
    /// </summary>
    public IEnumerable<CreateTestAnswerItem> Answers { get; set; } = Array.Empty<CreateTestAnswerItem>();
}