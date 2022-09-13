using Microsoft.EntityFrameworkCore;
using NTester.DataAccess.Entities;

namespace NTester.DataAccess.Data.NTesterDbContext;

/// <summary>
/// Database context of the NTester application.
/// </summary>
public interface INTesterDbContext
{
    /// <inheritdoc cref="DbSet{TEntity}"/>
    DbSet<TestEntity> Tests { get; set; }

    /// <inheritdoc cref="DbSet{QuestionEntity}"/>
    DbSet<QuestionEntity> Questions { get; set; }

    /// <inheritdoc cref="DbSet{AnswerEntity}"/>
    DbSet<AnswerEntity> Answers { get; set; }

    /// <inheritdoc cref="DbSet{ClientEntity}"/>
    public DbSet<ClientEntity> Clients { get; set; }

    /// <inheritdoc cref="DbSet{RefreshTokenEntity}"/>
    DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

    /// <inheritdoc cref="DbContext.SaveChangesAsync(System.Threading.CancellationToken)"/>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}