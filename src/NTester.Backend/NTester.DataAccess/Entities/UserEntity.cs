using Microsoft.AspNetCore.Identity;

namespace NTester.DataAccess.Entities;

/// <summary>
/// User entity to be stored in the database.
/// </summary>
public class UserEntity : IdentityUser<Guid>
{
    /// <summary>
    /// Name of the user.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Surname of the user.
    /// </summary>
    public string Surname { get; set; } = string.Empty;

    /// <summary>
    /// Tests of the user.
    /// </summary>
    public IEnumerable<TestEntity> Tests { get; set; } = new List<TestEntity>();

    /// <summary>
    /// Refresh tokens of the user.
    /// </summary>
    public IEnumerable<RefreshTokenEntity> RefreshTokens { get; set; } = new List<RefreshTokenEntity>();
}