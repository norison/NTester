﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTester.DataAccess.Entities;

namespace NTester.DataAccess.EntityTypeConfigurations;

/// <summary>
/// Configuration of the <see cref="TestEntity"/> in database.
/// </summary>
public class TestEntityTypeConfiguration : IEntityTypeConfiguration<TestEntity>
{
    /// <inheritdoc cref="IEntityTypeConfiguration{TestEntity}.Configure"/>
    public void Configure(EntityTypeBuilder<TestEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(200);
        builder.Property(x => x.CreationDateTime).HasDefaultValueSql("SYSUTCDATETIME()").IsRequired();
        builder.Property(x => x.Published).IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Tests)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}