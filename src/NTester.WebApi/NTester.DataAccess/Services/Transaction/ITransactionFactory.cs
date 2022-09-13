using Microsoft.EntityFrameworkCore.Storage;

namespace NTester.DataAccess.Services.Transaction;

/// <summary>
/// Factory of the database transactions.
/// </summary>
public interface ITransactionFactory
{
    /// <summary>
    /// Creates a database transaction.
    /// </summary>
    /// <returns>Database context transaction.</returns>
    Task<IDbContextTransaction> CreateTransactionAsync(CancellationToken cancellationToken);
}