using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTester.DataAccess.Entities;

namespace NTester.DataAccess.EntityTypeConfigurations;

/// <summary>
/// Configuration of the <see cref="QuestionEntity"/> in database.
/// </summary>
public class QuestionEntityTypeConfiguration : IEntityTypeConfiguration<QuestionEntity>
{
    /// <inheritdoc cref="IEntityTypeConfiguration{QuestionEntity}.Configure"/>
    public void Configure(EntityTypeBuilder<QuestionEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Content).HasMaxLength(100).IsRequired();

        builder
            .HasOne(x => x.Test)
            .WithMany(x => x.Questions)
            .HasForeignKey(x => x.TestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}