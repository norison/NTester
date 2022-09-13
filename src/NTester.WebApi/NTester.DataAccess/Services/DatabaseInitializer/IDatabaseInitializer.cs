using NTester.DataAccess.Data.NTesterDbContext;

namespace NTester.DataAccess.Services.DatabaseInitializer;

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