using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NTester.DataAccess.Entities;
using NTester.DataAccess.EntityTypeConfigurations;

namespace NTester.DataAccess.Data;

/// <inheritdoc cref="INTesterDbContext"/>
public class NTesterDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{
    /// <summary>
    /// Constructor of the NTester database context instance.
    /// </summary>
    /// <param name="options">Options of the database context.</param>
    public NTesterDbContext(DbContextOptions<NTesterDbContext> options) : base(options)
    {
    }

    /// <inheritdoc cref="INTesterDbContext.Tests"/>
    public DbSet<TestEntity> Tests { get; set; }

    /// <inheritdoc cref="INTesterDbContext.Questions"/>
    public DbSet<QuestionEntity> Questions { get; set; }

    /// <inheritdoc cref="INTesterDbContext.Answers"/>
    public DbSet<AnswerEntity> Answers { get; set; }

    /// <inheritdoc cref="DbContext.OnModelCreating"/>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new TestEntityTypeConfiguration());
        builder.ApplyConfiguration(new QuestionEntityTypeConfiguration());
        builder.ApplyConfiguration(new AnswerEntityTypeConfiguration());
    }
}