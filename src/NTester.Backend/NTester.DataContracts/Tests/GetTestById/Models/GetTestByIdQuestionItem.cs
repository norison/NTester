namespace NTester.DataContracts.Tests.GetTestById.Models;

/// <summary>
/// Question response item.
/// </summary>
public class GetTestByIdQuestionItem
{
    /// <summary>
    /// Id of the question.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Content of the question.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Answers of the question.
    /// </summary>
    public IEnumerable<GetTestByIdAnswerItem> Answers { get; set; } = Array.Empty<GetTestByIdAnswerItem>();
}