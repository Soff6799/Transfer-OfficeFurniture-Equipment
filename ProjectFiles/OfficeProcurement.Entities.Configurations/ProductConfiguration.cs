using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfficeProcurement.Context.EntityFrameworkCore;

namespace OfficeProcurement.Entities.Configurations;

/// <summary>
/// Конфигурация сущности <see cref="Product"/> для  Entity Framework Core
/// </summary>
public class ProductConfiguration: IEntityTypeConfiguration<Product>
{
    /// <summary>
    /// Настраивает правила отображения полей сущности, индексы и ограничения для таблицы продуктов
    /// </summary>
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");
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

        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
    }

}
