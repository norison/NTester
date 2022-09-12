namespace NTester.DataAccess.Data;

/// <summary>
/// Initializes the database.
/// </summary>
public interface IDatabaseInitializer
{
    /// <summary>
    /// Initializes a database based on <see cref="NTesterDbContext"/> configuration.
    /// </summary>
    /// <returns></returns>
    Task InitializeAsync();
}