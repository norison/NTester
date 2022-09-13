namespace NTester.DataAccess.Entities;

/// <summary>
/// Refresh token entity to be stored in the database.
/// </summary>
public class RefreshTokenEntity
{
    /// <summary>
    /// Token of the refresh token entity.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Expiration date time of the refresh token entity.
    /// </summary>
    public DateTime ExpirationDateTime { get; set; }

    /// <summary>
    /// Id of the user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// User of the refresh token entity.
    /// </summary>
    public UserEntity User { get; set; } = new();
}