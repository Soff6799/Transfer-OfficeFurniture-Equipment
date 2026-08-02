using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OfficeProcurement.Entities.Configurations;

/// <summary>
/// Конфигурация сущности <see cref="Client"/> для  Entity Framework Core
/// </summary>
public class ClientConfiguration: IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");
    }
}



