using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTester.DataAccess.Entities;

namespace NTester.DataAccess.EntityTypeConfigurations;

/// <summary>
/// Configuration of the <see cref="AnswerEntity"/> in database.
/// </summary>
[ExcludeFromCodeCoverage]
public class AnswerEntityTypeConfiguration : IEntityTypeConfiguration<AnswerEntity>
{
    /// <inheritdoc cref="IEntityTypeConfiguration{AnswerEntity}.Configure"/>
    public void Configure(EntityTypeBuilder<AnswerEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Content).HasMaxLength(100).IsRequired();

        builder
            .HasOne(x => x.Question)
            .WithMany(x => x.Answers)
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}