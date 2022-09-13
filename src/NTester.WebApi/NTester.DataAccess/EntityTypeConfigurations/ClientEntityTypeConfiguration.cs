using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTester.DataAccess.Entities;

namespace NTester.DataAccess.EntityTypeConfigurations;

/// <summary>
/// Configuration of the <see cref="ClientEntity"/> in database.
/// </summary>
public class ClientEntityTypeConfiguration : IEntityTypeConfiguration<ClientEntity>
{
    /// <inheritdoc cref="IEntityTypeConfiguration{ClientEntity}.Configure"/>
    public void Configure(EntityTypeBuilder<ClientEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired();
    }
}