namespace NTester.DataAccess.Entities;

/// <summary>
/// Test entity to be stored in the database.
/// </summary>
public class TestEntity
{
    /// <summary>
    /// Id of the entity.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Title of the entity.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description of the entity.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Questions of the test.
    /// </summary>
    public IEnumerable<QuestionEntity> Questions { get; set; } = new List<QuestionEntity>();
}