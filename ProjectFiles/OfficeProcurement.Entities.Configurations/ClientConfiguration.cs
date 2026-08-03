using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OfficeProcurement.Entities.Configurations;

/// <summary>
/// Конфигурация сущности <see cref="Client"/> для  Entity Framework Core
/// </summary>
public class ClientConfiguration: IEntityTypeConfiguration<Client>
{
    /// <summary>
    /// Настраивает правила отображения полей сущности, индексы и ограничения для таблицы клиентов
    /// </summary>
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt)
            .IsRequired();
        builder.Property(x => x.CreatedBy)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.UpdatedAt)
            .IsRequired();
        builder.Property(x => x.UpdatedBy)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.CompanyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ContactLastName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ContactName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ContactPatronymic)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(x => x.CeoBasis)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Inn)
            .IsRequired()
            .HasMaxLength(12);

        builder.Property(x => x.LegalAddress)
            .IsRequired()
            .HasMaxLength(400);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasIndex(x => x.Inn)
            .HasDatabaseName("IX_Clients_Inn")
            .IsUnique();

        builder.HasIndex(x => x.ContactLastName)
            .HasDatabaseName("IX_Clients_ContactLastName");


    }

}

