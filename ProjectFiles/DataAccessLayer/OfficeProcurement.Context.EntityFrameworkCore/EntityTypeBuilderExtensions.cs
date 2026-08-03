using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OfficeProcurement.Dal.Contracts;

namespace OfficeProcurement.Context.EntityFrameworkCore;

public static class EntityTypeBuilderExtensions
{
    /// <summary>
    /// Задаёт конфигурацию ключа для идентификатора <see cref="IEntityWithId"/>
    /// </summary>
    public static void HasIdAsKey<T>(this EntityTypeBuilder<T> builder)
        where T : class, IEntityWithId
        => builder.HasKey(x => x.Id);

    /// <summary>
    /// Задаёт конфигурайию свойств аудита добавления для сущности <inheritdoc cref="BaseAuditEntity"/>
    /// </summary>
    public static void CreateAuditConfiguration<T>(this EntityTypeBuilder<T> builder)
        where T : class, IEntityAuditCreated
    {
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(200);
    }
}
