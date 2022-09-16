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

        builder.HasData(new List<ClientEntity>
        {
            new()
            {
                Id = Guid.Parse("5F730EAF-544E-423A-B24B-59A37A3155D6"),
                Name = "Postman"
            },
            new()
            {
                Id = Guid.Parse("14147A39-737F-49AF-B75D-44EBA6B61885"),
                Name = "NTester Web App"
            }
        });
    }
}