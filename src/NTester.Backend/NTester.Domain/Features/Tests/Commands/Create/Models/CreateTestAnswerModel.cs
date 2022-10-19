namespace NTester.Domain.Features.Tests.Commands.Create.Models;

/// <summary>
/// Model of the answer to create a test.
/// </summary>
public class CreateTestAnswerModel
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