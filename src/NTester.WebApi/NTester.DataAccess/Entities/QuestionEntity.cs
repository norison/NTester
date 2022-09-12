namespace NTester.DataAccess.Entities;

/// <summary>
/// Question entity to be stored in the database.
/// </summary>
public class QuestionEntity
{
    /// <summary>
    /// Id of the entity.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Content of the question.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Id of the test.
    /// </summary>
    public Guid TestId { get; set; }

    /// <summary>
    /// Test of the question.
    /// </summary>
    public TestEntity Test { get; set; } = new();

    /// <summary>
    /// Answers of the question.
    /// </summary>
    public IEnumerable<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();
}