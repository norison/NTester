using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTester.DataAccess.Entities;

namespace NTester.DataAccess.EntityTypeConfigurations;

/// <summary>
/// Configuration of the <see cref="RefreshTokenEntity"/> in database.
/// </summary>
public class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
    /// <inheritdoc cref="IEntityTypeConfiguration{RefreshTokenEntity}.Configure"/>
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.HasKey(x => x.Token);

        builder
            .HasIndex(x => new { x.UserId, x.ClientId })
            .IsUnique()
            .IncludeProperties(x => new { x.Token, x.ExpirationDateTime });

        builder
            .HasOne(x => x.Client)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}