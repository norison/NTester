using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace NTester.DataAccess.Services.DatabaseInitializer;

/// <summary>
/// Initializes the database.
/// </summary>
public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly Data.NTesterDbContext.NTesterDbContext _dbContext;
    private readonly ClientPresets _clientPresets;

    /// <summary>
    /// Create an instance of the database initializer.
    /// </summary>
    /// <param name="dbContext">Database context of the NTester application.</param>
    /// <param name="clientPresets">Presets of the clients.</param>
    public DatabaseInitializer(Data.NTesterDbContext.NTesterDbContext dbContext, IOptions<ClientPresets> clientPresets)
    {
        _dbContext = dbContext;
        _clientPresets = clientPresets.Value;
    }

    /// <inheritdoc cref="IDatabaseInitializer.InitializeAsync"/>
    public async Task InitializeAsync()
    {
        await _dbContext.Database.MigrateAsync();

        foreach (var client in _clientPresets.Clients)
        {
            if (!_dbContext.Clients.Contains(client))
            {
                await _dbContext.Clients.AddAsync(client);
            }
        }

        await _dbContext.SaveChangesAsync();
    }
}