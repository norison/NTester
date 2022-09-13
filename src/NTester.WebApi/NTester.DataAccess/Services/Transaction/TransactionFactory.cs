using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Storage;
using NTester.DataAccess.Data.NTesterDbContext;

namespace NTester.DataAccess.Services.Transaction;

/// <inheritdoc cref="ITransactionFactory"/>
[ExcludeFromCodeCoverage]
public class TransactionFactory : ITransactionFactory
{
    private readonly NTesterDbContext _dbContext;

    /// <summary>
    /// Creates an instance of the transaction factory.
    /// </summary>
    /// <param name="dbContext">Database context of the application.</param>
    public TransactionFactory(NTesterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc cref="ITransactionFactory.CreateTransactionAsync"/>
    public async Task<IDbContextTransaction> CreateTransactionAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }
}