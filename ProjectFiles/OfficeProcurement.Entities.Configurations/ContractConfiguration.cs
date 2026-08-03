using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfficeProcurement.Context.EntityFrameworkCore;

namespace OfficeProcurement.Entities.Configurations;

/// <summary>
/// Конфигурация сущности <see cref="Contract"/> для  Entity Framework Core
/// </summary>
public class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    /// <summary>
    /// Настраивает правила отображения полей сущности, индексы
    /// и ограничения для таблицы договоров
    /// </summary>
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.ToTable("Contracts");
        builder.HasIdAsKey();
        builder.CreateAuditConfiguration();
        builder.UpdateAuditConfiguration();

        builder.Property(x => x.Number)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.SignedAt)
            .IsRequired();

        builder.Property(x => x.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Number)
            .HasDatabaseName("IX_Contracts_Number")
            .IsUnique();
    }
}
