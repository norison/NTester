using Microsoft.EntityFrameworkCore;

namespace NTester.DataAccess.Data.DatabaseInitializer;

/// <summary>
/// Initializes the database.
/// </summary>
public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly NTesterDbContext.NTesterDbContext _dbContext;

    /// <summary>
    /// Create an instance of the database initializer.
    /// </summary>
    /// <param name="dbContext">Database context of the NTester application.</param>
    public DatabaseInitializer(NTesterDbContext.NTesterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc cref="IDatabaseInitializer.InitializeAsync"/>
    public async Task InitializeAsync()
    {
        await _dbContext.Database.MigrateAsync();
    }
}