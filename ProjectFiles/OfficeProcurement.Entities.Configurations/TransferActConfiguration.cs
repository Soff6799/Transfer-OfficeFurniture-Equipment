using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfficeProcurement.Context.EntityFrameworkCore;

namespace OfficeProcurement.Entities.Configurations;

/// <summary>
/// Конфигурация сущности <see cref="TransferAct"/> для  Entity Framework Core
/// </summary>
public class TransferActConfiguration: IEntityTypeConfiguration<TransferAct>
{
    /// <summary>
    /// Настраивает правила отображения полей сущности, индексы и ограничения для таблицы Акта
    /// </summary>
    public void Configure(EntityTypeBuilder<TransferAct> builder)
    {
        builder.ToTable("TransferActs");
        builder.HasIdAsKey();
        builder.CreateAuditConfiguration();
        builder.UpdateAuditConfiguration();

        builder.Property(x => x.Number)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Date)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Number)
            .HasDatabaseName("IX_Clients_Inn")
            .IsUnique();
    }

}
