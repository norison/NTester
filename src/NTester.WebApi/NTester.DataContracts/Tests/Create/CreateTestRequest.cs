using NTester.DataContracts.Tests.Create.Models;

namespace NTester.DataContracts.Tests.Create;

/// <summary>
/// Request to create a test.
/// </summary>
public class CreateTestRequest
{
    /// <summary>
    /// Title of the test.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description of the test.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Questions of the test.
    /// </summary>
    public IEnumerable<CreateTestQuestionItem> Questions { get; set; } = Array.Empty<CreateTestQuestionItem>();
}