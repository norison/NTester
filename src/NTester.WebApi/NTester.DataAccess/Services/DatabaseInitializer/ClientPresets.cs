using NTester.DataAccess.Entities;

namespace NTester.DataAccess.Services.DatabaseInitializer;

/// <summary>
/// Contains presents of the clients.
/// </summary>
public class ClientPresets
{
    /// <summary>
    /// List of the clients.
    /// </summary>
    public IEnumerable<ClientEntity> Clients { get; set; }
}