namespace NTester.DataAccess.Entities;

/// <summary>
/// Client entity to be stored in the database.
/// </summary>
public class ClientEntity
{
    /// <summary>
    /// Name of the client.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Refresh tokens of the client.
    /// </summary>
    public IEnumerable<RefreshTokenEntity> RefreshTokens { get; set; } = new List<RefreshTokenEntity>();
}