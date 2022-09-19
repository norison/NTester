using MediatR;
using NTester.DataContracts.Tests.Create;
using NTester.Domain.Features.Tests.Commands.Create.Models;

namespace NTester.Domain.Features.Tests.Commands.Create;

/// <summary>
/// Command to create a test.
/// </summary>
public class CreateTestCommand : IRequest<CreateTestResponse>
{
    /// <summary>
    /// Id of the user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Title of the test.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description of the test.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Questions of the test.
    /// </summary>
    public IEnumerable<CreateTestQuestionModel> Questions { get; set; } = Array.Empty<CreateTestQuestionModel>();
}