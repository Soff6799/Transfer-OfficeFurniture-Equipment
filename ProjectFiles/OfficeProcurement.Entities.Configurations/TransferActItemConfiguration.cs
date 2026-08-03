using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfficeProcurement.Context.EntityFrameworkCore;

namespace OfficeProcurement.Entities.Configurations;

/// <summary>
/// Конфигурация сущности <see cref="TransferActItem"/> для  Entity Framework Core
/// </summary>
public class TransferActItemConfiguration: IEntityTypeConfiguration<TransferActItem>
{
    /// <summary>
    /// Настраивает правила отображения полей сущности,
    /// индексы и ограничения для таблицы содержимого акта
    /// </summary>
    public void Configure(EntityTypeBuilder<TransferActItem> builder)
    {
        builder.ToTable("TransferActItems");
        builder.HasIdAsKey();
        builder.CreateAuditConfiguration();
        builder.UpdateAuditConfiguration();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.StockNumber)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(x => x.Specifications)
            .IsRequired(false)
            .HasMaxLength(300);

        builder.Property(x => x.Unit)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.Quantity)
            .IsRequired()
            .HasColumnType("decimal(8,3)");

        builder.Property(x => x.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasIndex(x => x.StockNumber)
            .HasDatabaseName("IX_TransferActItems_StockNumber")
            .IsUnique();

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_TransferActItems_Name");
    }


}
