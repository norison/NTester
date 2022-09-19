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
        builder.HasKey(x => x.Name);

        builder.HasData(new List<ClientEntity>
        {
            new()
            {
                Name = "Postman"
            },
            new()
            {
                Name = "Swagger"
            },
            new()
            {
                Name = "NTester Web App"
            }
        });
    }
}