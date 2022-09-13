using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTester.DataAccess.Entities;

namespace NTester.DataAccess.EntityTypeConfigurations;

/// <summary>
/// Configuration of the <see cref="UserEntity"/> in database.
/// </summary>
[ExcludeFromCodeCoverage]
public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
{
    /// <inheritdoc cref="IEntityTypeConfiguration{UserEntity}.Configure"/>
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(10).IsRequired();
        builder.Property(x => x.Surname).HasMaxLength(10).IsRequired();
    }
}