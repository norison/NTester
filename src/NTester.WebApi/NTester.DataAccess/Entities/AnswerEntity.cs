namespace NTester.DataAccess.Entities;

/// <summary>
/// Answer entity to be stored in the database.
/// </summary>
public class AnswerEntity
{
    /// <summary>
    /// Id of the entity.
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

    /// <summary>
    /// Id of the question.
    /// </summary>
    public Guid QuestionId { get; set; }

    /// <summary>
    /// Question of the answer.
    /// </summary>
    public QuestionEntity? Question { get; set; }
}