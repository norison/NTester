namespace NTester.Domain.Features.Tests.Commands.Create.Models;

/// <summary>
/// Model of the question to create a test.
/// </summary>
public class CreateTestQuestionModel
{
    /// <summary>
    /// Content of the question.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Answers of the question.
    /// </summary>
    public IEnumerable<CreateTestAnswerModel> Answers { get; set; } = Array.Empty<CreateTestAnswerModel>();
}