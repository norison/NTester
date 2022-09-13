using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTester.DataAccess.Entities;

namespace NTester.DataAccess.EntityTypeConfigurations;

/// <summary>
/// Configuration of the <see cref="TestEntity"/> in database.
/// </summary>
[ExcludeFromCodeCoverage]
public class TestEntityTypeConfiguration : IEntityTypeConfiguration<TestEntity>
{
    /// <inheritdoc cref="IEntityTypeConfiguration{TestEntity}.Configure"/>
    public void Configure(EntityTypeBuilder<TestEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(200).IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Tests)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}