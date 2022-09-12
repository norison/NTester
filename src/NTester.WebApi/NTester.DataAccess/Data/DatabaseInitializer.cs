using Microsoft.EntityFrameworkCore;

namespace NTester.DataAccess.Data;

/// <summary>
/// Initializes the database.
/// </summary>
public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly NTesterDbContext _dbContext;

    /// <summary>
    /// Create an instance of the database initializer.
    /// </summary>
    /// <param name="dbContext">Database context of the NTester application.</param>
    public DatabaseInitializer(NTesterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc cref="IDatabaseInitializer.InitializeAsync"/>
    public async Task InitializeAsync()
    {
        await _dbContext.Database.MigrateAsync();
    }
}